import { PaginationParams } from "../interfaces";

export class GetLogs {
  static readonly type = '[Logs] Get logs';
  constructor(public paginationParams: PaginationParams) {}
}