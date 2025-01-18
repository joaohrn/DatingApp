import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
	const router = inject(Router);
	const toastr = inject(ToastrService);

	return next(req).pipe(
		catchError((error) => {
			switch (error.status) {
				case 400:
					const errors = error.error.errors;
					if (errors) {
						const modalStateErrors = [];
						for (const key in errors) {
							if (errors[key]) {
								modalStateErrors.push(errors[key]);
							}
						}
						throw modalStateErrors.flat();
					} else {
						toastr.error(error.error, error.status);
					}
					break;
				case 401:
					toastr.error('Unauthorized', error.status);
					break;
				case 404:
					router.navigateByUrl('/not-found');
					break;
				case 500:
					const navigationExtras: NavigationExtras = {
						state: { error: error.error },
					};
					router.navigateByUrl('/server-error', navigationExtras);
					break;
				default:
					toastr.error('Something unnexpected went wrong');
					break;
			}
			throw error;
		})
	);
};
