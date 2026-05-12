import {
  Component,
  computed,
  effect,
  inject,
  OnInit,
  signal,
  ViewChild,
  viewChild,
} from '@angular/core';
import { UserService } from '../../core/services/user.service';
import { User } from '../../models/user.model';
import { RouterModule } from '@angular/router';
import { MatDialog, MatDialogTitle } from '@angular/material/dialog';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule, MatIconAnchor, MatIconButton } from '@angular/material/button';
import { UserDialogComponent } from '../user-dialog-component/user-dialog.component';
import { CreateUserDto } from '../../models/createUser.dto';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { ConfirmDialog } from '../../shared/confirm-dialog/confirm-dialog';
import { MatFormField, MatLabel } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldControl, MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-manage-users-component',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatButtonModule,
    MatTableModule,
    MatDialogTitle,
    MatPaginatorModule,
    MatIconModule,
    MatFormFieldModule,
  ],
  templateUrl: './manage-users.component.html',
  styleUrl: './manage-users.component.scss',
})
export class ManageUsersComponent implements OnInit {
  users = signal<User[]>([]);
  displayedColumns = ['email', 'role', 'createdAt', 'isActive', 'actions'];
  userService = inject(UserService);
  dialog = inject(MatDialog);
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  dataSource = new MatTableDataSource<User>();

  ngOnInit(): void {
    this.loadUsers();
  }

  constructor() {
    effect(() => {
      this.dataSource.data = this.users();

      this.dataSource.paginator = this.paginator;
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;

    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  loadUsers() {
    return this.userService.getAll().subscribe((data) => {
      this.users.set(data);
    });
  }

  toggleIsActiveUser(user: User) {
    this.userService.toggleIsActiveUser(user).subscribe((res) => {
      if (res) {
        this.loadUsers();
      }
    });
  }

  openCreateDialog() {
    const dialogRef = this.dialog.open(UserDialogComponent, { data: { isEditMode: false } });

    dialogRef.afterClosed().subscribe((data) => {
      this.userService.create(data as CreateUserDto).subscribe({
        next: (data) => {
          this.loadUsers();
        },
        error: (err) => {
          console.error(err);
        },
      });
    });
  }

  deleteUser(user: User) {
    const dialogRef = this.dialog.open(ConfirmDialog, {
      data: { title: 'Delete user', message: `Do you want to delete user: "${user.email}"?` },
    });

    dialogRef.afterClosed().subscribe((data) => {
      if (data == true) {
        this.userService.delete(user).subscribe((res) => {
          if (res) this.loadUsers();
        });
      }
    });
  }
}
