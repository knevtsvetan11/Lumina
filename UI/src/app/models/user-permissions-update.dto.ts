export interface UserPermissionUpdateDto {
  userId: string;
  permissions: {
    permissionName: string;
    isGranted: boolean;
  }[];
}