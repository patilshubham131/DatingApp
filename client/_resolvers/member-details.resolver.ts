import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { MembersService } from 'src/app/_services/members.service';

@Injectable({
    providedIn:"root"
})
export class MemberDetailsResolver implements Resolve<Member>{


    constructor(private memberService: MembersService){

    }

    resolve(route: ActivatedRouteSnapshot):Observable<Member> {
        console.log(route.paramMap);
        return this.memberService.getMember(route.paramMap.get('username'));
    }

}