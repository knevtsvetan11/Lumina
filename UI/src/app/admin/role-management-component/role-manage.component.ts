import { Component, inject, Inject, OnInit, signal } from '@angular/core';
import { RoleService } from '../../core/services/role.service';
import { Role } from '../../models/role.modes';
import { PermissionService } from '../../core/services/permission.service';
import { Permission } from '../../models/permission.model';
import { MatCardActions, MatCardHeader, MatCardModule } from '@angular/material/card';
import { MatCheckbox, MatCheckboxModule } from '@angular/material/checkbox';
import { MatIcon } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { CreateRoleDialogComponent } from './create-role-dialog/create-role-dialog.component';
import { catchError, filter, switchMap } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfirmDialog } from '../../shared/confirm-dialog/confirm-dialog';

@Component({
  selector: 'app-role-management-component',
  standalone: true,
  imports: [
    MatCardModule,
    MatCheckboxModule,
    MatCardHeader,
    MatIcon,
    MatToolbarModule,
    MatCardActions,
    MatButtonModule,
  ],
  templateUrl: './role-manage.component.html',
  styleUrl: './role-manage.component.scss',
})
export class RoleManageComponent implements OnInit {
  snackBar = inject(MatSnackBar);
  dialog = inject(MatDialog);
  roleService = inject(RoleService);
  permissionService = inject(PermissionService);
  roles = signal<Role[]>([]);
  selectedPermission = signal<string[]>([]);
  selectedRoleId = signal<string>('');

  allPermissions = signal<string[]>([]);

  ngOnInit(): void {
    this.loadAllRoles();
    this.loadAllPermission();
  }

  openDeleteDialog(role: Role) {
    const roleId = role.id;
    const dialogRef = this.dialog
      .open(ConfirmDialog, {
        data: { title: `Delete`, message: `Do you want to delete: ${role.name}?` },
      })
      .afterClosed()
      .pipe(
        filter((confirmed) => !!confirmed),
        switchMap(() => this.roleService.delete(role.id)),
      )
      .subscribe({
        next: (res) => {
          (this.roles.update((roles) => roles.filter((r) => r.id !== role.id)),
            this.snackBar.open('Succeslly deleted role.'));
        },
        error: (err) => {
          this.snackBar.open('Error to delete this role');
        },
      });
  }

  openCreateRoleDialog() {
    const dialogRef = this.dialog
      .open(CreateRoleDialogComponent)
      .afterClosed()
      .pipe(
        filter((dto) => !!dto),
        switchMap((dto) => this.roleService.createRole(dto)),
      )
      .subscribe((newRole) => {
        if (newRole.id != null) this.roles.update((roles) => [newRole, ...roles]);
        this.snackBar.open('Succeslly created new role.');
      });
  }

  loadAllRoles() {
    this.roleService.getAllRoles().subscribe((res) => {
      if (res != null) this.roles.set(res);
    });
  }

  loadAllPermission() {
    this.permissionService.getAllPermissions().subscribe((data) => {
      if (data != null) this.allPermissions.set(data);
    });
  }

  getRolePermissions(roleId: string) {
    this.permissionService.getPermissionsByRoleId(roleId).subscribe((perm) => {
      if (perm != null) {
        this.selectedPermission.set(perm);
        this.selectedRoleId.set(roleId);
      }
    });
  }

  onChangePermission(perm: string, isChacked: boolean) {
    if (!isChacked) {
      this.selectedPermission().filter((p) => p !== perm);
    } else this.selectedPermission.update((prev) => [...prev, perm]);
  }

  updateRolePermissions(roleId: string) {
    if (!roleId) return;

    this.permissionService.update(roleId, this.selectedPermission()).subscribe({
      next: () => {
        this.snackBar.open('Succeslly save new permissions on this role.');
      },
      error: () => {
        console.error();
      },
    });
  }
}
