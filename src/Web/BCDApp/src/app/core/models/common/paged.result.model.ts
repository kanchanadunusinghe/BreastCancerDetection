export interface PagedResultModel<T> {
	pageNumber: number;
	pageSize: number;
	totalCount: number;
	data: T[];
}
