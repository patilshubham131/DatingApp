import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Member } from '../_models/member';
import { map, take } from 'rxjs/operators';
import { of } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { User } from '../_models/User';
import { Message } from '../_models/message';

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
  memberCache= new Map();
  user: User;
  userparams: UserParams;

  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user=>{
      this.user = user;
      this.userparams = new UserParams(user);
    })
   }

   getUserParams(){
     return this.userparams;
   }

   setUserParams(params){
     this.userparams = params;
   }

   resetUserParams(){
     this.userparams = new UserParams(this.user);
     return this.userparams; 
   }

  //old method to get member
  // getMembers(page ?: number, itemsPerPage?: number){
  //   // if(this.members.length>0) 
  //   //   return of(this.members);

  //   // return this.http.get<Member[]>(this.baseUrl + "users", httpoptions).pipe(
  //   //   map(members=>{
  //   //     this.members = members;
  //   //     return members;
  //   //   })
  //   // );

  //   let params = new HttpParams();

  //   if(page !== null && itemsPerPage !== null){
  //     params = params.append('pageNumber', page.toString());
  //     params = params.append('pageSize', itemsPerPage.toString());
  //   }
  //   return this.http.get<Member[]>(this.baseUrl + "users", {observe: 'response', params}).pipe(
  //       map(response=>{
  //         this.paginatedResult.result = response.body;
  //         if(response.headers.get('Pagination') !== null){
  //           this.paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
  //         }
  //         return this.paginatedResult;
  //       })
  //     );

  // }


  getMembers(userParams: UserParams){
   
    console.log(Object.values(userParams).join('-'));
    let res = this.memberCache.get(Object.values(userParams).join('-'));
    
    if(res){
      return of(res);
    }

    let params = this.getPaginationHeader(userParams.pageNumber, userParams.pageSize);

    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users',params).pipe(
      (map( response=>{
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      }))
    );

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

  getMember(username: string){
    //var member = this.members.find(x=> x.userName === username);

    // if(this.members !== undefined){
    //   return of(member);
    // }

    let member = [...this.memberCache.values()]
    .reduce((arr, elem)=>arr.concat(elem.result),[])
    .find((member: Member)=> member.userName === username);

    if(member){
      return of(member);
    }

    console.log(member);
    return this.http.get<Member>(this.baseUrl +"users/"+username);
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

  addLike(username: string){
    return this.http.post(this.baseUrl + "likes/" + username,{});
  }

  getLikes(predicate: string, pageNumber, pageSize){

    let params = this.getPaginationHeader(pageNumber,pageSize);
    params = params.append('predicate',predicate);

    // return this.http.get<Partial<Member[]>>(this.baseUrl + "likes?predicate=" + predicate);

    return this.getPaginatedResult<Partial<Member[]>>(this.baseUrl + 'likes', params);
  }

}
