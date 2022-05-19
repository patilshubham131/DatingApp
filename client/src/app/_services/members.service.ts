import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Member } from '../_models/member';
import { map } from 'rxjs/operators';
import { of } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';

const httpoptions = {
  headers:new HttpHeaders({
    Authorization: "Bearer "+ JSON.parse(localStorage.getItem('user'))?.token
  })
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl = environment.apiUrl;
  members: Member[] = [];
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

  constructor(private http: HttpClient) { }

  getMembers(page ?: number, itemsPerPage?: number){
    // if(this.members.length>0) 
    //   return of(this.members);

    // return this.http.get<Member[]>(this.baseUrl + "users", httpoptions).pipe(
    //   map(members=>{
    //     this.members = members;
    //     return members;
    //   })
    // );

    let params = new HttpParams();

    if(page !== null && itemsPerPage !== null){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }
    return this.http.get<Member[]>(this.baseUrl + "users", {observe: 'response', params}).pipe(
        map(response=>{
          this.paginatedResult.result = response.body;
          if(response.headers.get('Pagination') !== null){
            this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return this.paginatedResult;
        })
      );

  }

  getMember(username: string){
    var member = this.members.find(x=> x.userName === username);

    if(this.members !== undefined){
      return of(member);
    }
    return this.http.get<Member>(this.baseUrl +"users/"+username, httpoptions);
  }

  updateMember(member: Member){
    return this.http.put(this.baseUrl + "users",member).pipe(
      map(()=>{
        var index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto(photoId: number){
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number){
    return this.http.delete(this.baseUrl + 'users/delete-photo/'+ photoId);
  }
}
