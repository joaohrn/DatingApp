import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { User } from '../_models/user';
import { environment } from '../../environments/environment';
import { LikesService } from './likes.service';

@Injectable({
	providedIn: 'root',
})
export class AccountService {
	private http = inject(HttpClient);
	private likesService = inject(LikesService);
	baseUrl: string = environment.apiUrl;
	currentUser = signal<User | null>(null);
	login(model: any) {
		return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
			map((user) => {
				if (user) {
					this.setCurrentUser(user);
				}
			})
		);
	}
	register(model: any) {
		return this.http
			.post<User>(this.baseUrl + 'account/register', model)
			.pipe(
				map((user) => {
					if (user) {
						this.setCurrentUser(user);
					}
					return user;
				})
			);
	}
	setCurrentUser(user: User) {
		localStorage.setItem('user', JSON.stringify(user));
		this.currentUser.set(user);
		this.likesService.getLikeIds();
	}
	logout() {
		localStorage.removeItem('user');
		this.currentUser.set(null);
	}
}
