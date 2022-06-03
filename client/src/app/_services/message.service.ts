import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getmessage(pagenumber, pagesize, container){
    let params = this.getPaginationHeader(pagenumber, pagesize);

    params = params.append('Container', container);

    return this.getPaginatedResult<Message[]>(this.baseUrl + "messages", params);
  }

  
  getMessageThread(username: string){
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }

  
  sendMessage(username: string, content: string){
    return this.http.post<Message>(this.baseUrl + "messages",{recipientUsername: username, content})
  }

  deleteMessage(id: number){
    return this.http.delete(this.baseUrl + "messages/" + id);
  }

  private getPaginatedResult<T>(url, params) {

    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
          paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeader(pagenumber: number, pageSize: number){
    
    let params = new HttpParams();

      params = params.append('pageNumber', pagenumber.toString());
      params = params.append('pageSize', pageSize.toString());
    
      return params;
  }

}
