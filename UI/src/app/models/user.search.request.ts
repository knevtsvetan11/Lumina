export interface UserSearchRequest {
  pageIndex: number;
  pageSize: number;
  searchData: string | null;
  filterColumn: string | null;
}