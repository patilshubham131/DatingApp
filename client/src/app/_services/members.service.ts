import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Member } from '../_models/member';
import { map } from 'rxjs/operators';
import { of } from 'rxjs';

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

  constructor(private http: HttpClient) { }

  getMembers(){
    if(this.members.length>0) 
      return of(this.members);

    return this.http.get<Member[]>(this.baseUrl + "users", httpoptions).pipe(
      map(members=>{
        this.members = members;
        return members;
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
}
