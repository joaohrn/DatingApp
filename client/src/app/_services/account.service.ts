import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { map, Observable } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
	providedIn: 'root',
})
export class AccountService {
	http = inject(HttpClient);
	baseUrl: string = 'https://localhost:5001/api/';
	currentUser = signal<User | null>(null);
	login(model: any) {
		return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
			map((user) => {
				if (user) {
					localStorage.setItem('user', JSON.stringify(user));
					this.currentUser.set(user);
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
						localStorage.setItem('user', JSON.stringify(user));
						this.currentUser.set(user);
					}
					return user;
				})
			);
	}
	logout() {
		localStorage.removeItem('user');
		this.currentUser.set(null);
	}
}
