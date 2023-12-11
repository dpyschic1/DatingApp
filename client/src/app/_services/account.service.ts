import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl; // Get the url for api
  private currentUserSource = new BehaviorSubject<User | null>(null); // create a behaviour object to store the user
  currentUser$ = this.currentUserSource.asObservable(); //create an observable


  constructor(private http: HttpClient, private presenceService: PresenceService) { }
  login(model: User){
    return this.http.post<User>(this.baseUrl  + 'account/login', model).pipe( //use pipe and map to transform and pass the user value
      map((response:any) => {
        const user = response;
        if(user){
           this.setCurrentUser(user);
        }
      })
    )
  }

  register(model : any){
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
        }
      })

    )
  }

  setCurrentUser(user: User){
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push();
    localStorage.setItem('user',JSON.stringify(user)); //convert the user to string and save it in local storage
    this.currentUserSource.next(user); //method to set the current user
    this.presenceService.createHubConnection(user);
  }

  logout(){
    localStorage.removeItem('user'); //remove the user from local storage and behaviour object
    this.currentUserSource.next(null);
    this.presenceService.stopHubConnection();
  }

  getDecodedToken(token: string){
    return JSON.parse(atob(token.split('.')[1]))
  }
  


}
