import { Component, inject, NgModule, OnInit, signal, ViewChild, viewChild } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { User } from '../../models/user.model';
import { UserService } from '../../core/services/user.service';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserSearchRequest } from '../../models/user.search.request';
import { MatOptionModule, MatOptionSelectionChange } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatCardHeader, MatCardModule } from '@angular/material/card';
import { MatHeaderRow } from '@angular/material/table';
import { MatCheckboxChange, MatCheckboxModule } from '@angular/material/checkbox';
import { PermissionService } from '../../core/services/permission.service';
import { Permission } from '../../models/permission.model';
import { UserPermissionDto } from '../../models/user-permissions.dto';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-special-permissions-component',
  standalone: true,
  imports: [
    FormsModule,
    MatToolbarModule,
    MatPaginatorModule,
    MatInputModule,
    ReactiveFormsModule,
    MatSelectModule,
    MatOptionModule,
    MatCardModule,
    MatCheckboxModule,
    MatButtonModule,
  ],
  templateUrl: './special-permissions.component.html',
  styleUrl: './special-permissions.component.scss',
})
export class SpecialPermissionsComponent implements OnInit {
  filterOptions = [
    { value: 'all', label: 'All' },
    { value: 'username', label: 'Username' },
    { value: 'email', label: 'Email' },
    { value: 'firstname', label: 'FirstName' },
    { value: 'lastname', label: 'LastName' },
    { value: 'phone', label: 'Phone' },
    { value: 'id', label: 'Id' },
    { value: 'address', label: 'Adress' },
  ];

  selectedOption = signal<string>('');
  usersPaged = signal<User[]>([]);
  userPermissions = signal<UserPermissionDto[]>([]);
  userService = inject(UserService);
  permissionService = inject(PermissionService);
  totalCount = signal<number>(0);
  selectedUserId = signal<string>('');
  snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  pageIndex = signal<number>(0);
  pageSize = signal<number>(10);
  searchControl = new FormControl('');

  ngOnInit(): void {
    this.loadUsersPaged();
  }

  savePermissions() {
    const payload = {
      userId: this.selectedUserId(),
      permissions: this.userPermissions()
        .filter((p) => p.isGranted && !p.isInheritedFromRole)
        .map((p) => p.permissionValue.trim()),
    };

    this.permissionService.saveUserPermissions(payload).subscribe({
      next: () => {
        this.snackBar.open('Successfully saved user permissions!', 'OK', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
        });
      },
      error: (err) => {
        console.error('Save failed:', err);
        this.snackBar.open('Failed to save permissions. Please try again.', 'Close', {
          duration: 5000,
        });
      },
    });
  }

  loadUserPermissions(userId: string) {
    if (userId !== null) this.selectedUserId.set(userId);

    this.permissionService.getUserPermissions(userId).subscribe((dto) => {
      if (dto != null) {
        this.userPermissions.set(dto);
      }
    });
  }

  onSearch() {
    if (this.searchControl.valid) {
      this.paginator.pageIndex = 0;
      this.loadUsersPaged();
    }
  }

  loadUsersPaged() {
    const requestDto: UserSearchRequest = {
      pageIndex: this.paginator ? this.paginator.pageIndex : 0,
      pageSize: this.paginator ? this.paginator.pageSize : 10,
      searchData: this.searchControl.value,
      filterColumn: this.selectedOption(),
    };

    this.userService.getUsersPaged(requestDto).subscribe((data) => {
      if (data.length > 0) {
        this.usersPaged.set(data);
        this.totalCount.set(data[0].countUsers);
      }
    });
  }
}
