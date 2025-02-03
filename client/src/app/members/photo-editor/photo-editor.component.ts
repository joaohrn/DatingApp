import { Component, inject, input, OnInit, output } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/account.service';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { FileUploader, FileUploadModule } from 'ng2-file-upload';
import { environment } from '../../../environments/environment';
import { Photo } from '../../_models/photo';
import { MembersService } from '../../_services/members.service';

@Component({
	selector: 'app-photo-editor',
	standalone: true,
	imports: [NgClass, NgIf, NgFor, DecimalPipe, NgStyle, FileUploadModule],
	templateUrl: './photo-editor.component.html',
	styleUrl: './photo-editor.component.css',
})
export class PhotoEditorComponent implements OnInit {
	private accountService = inject(AccountService);
	private membersService = inject(MembersService);
	member = input.required<Member>();
	hasBaseDropZoneOver = false;
	baseUrl = environment.apiUrl;
	uploader?: FileUploader;
	memberChange = output<Member>();
	ngOnInit(): void {
		this.initializeUploader();
	}
	fileOverBase($event: any) {
		this.hasBaseDropZoneOver = $event;
	}
	initializeUploader() {
		this.uploader = new FileUploader({
			url: this.baseUrl + 'users/add-photo',
			isHTML5: true,
			authToken: 'bearer ' + this.accountService.currentUser()?.token,
			allowedFileType: ['image'],
			removeAfterUpload: true,
			autoUpload: false,
			maxFileSize: 10 * 1024 * 1024,
		});
		this.uploader.onAfterAddingFile = (file) => {
			file.withCredentials = false;
		};
		this.uploader.onSuccessItem = (item, response, status, headers) => {
			const photo = JSON.parse(response);
			const memberUpload = { ...this.member() };
			memberUpload.photos.push(photo);
			this.memberChange.emit(memberUpload);
		};
	}
	setMainPhoto(photo: Photo) {
		this.membersService.setMainPhoto(photo).subscribe({
			next: (_) => {
				const user = this.accountService.currentUser();
				if (user) {
					user.photoUrl = photo.url;
					this.accountService.setCurrentUser(user);
				}
				const updatedMember = { ...this.member() };
				updatedMember.photoUrl = photo.url;
				updatedMember.photos.forEach((p) => {
					if (p.isMain) p.isMain = false;
					if (p.id == photo.id) p.isMain = true;
				});
				this.memberChange.emit(updatedMember);
			},
		});
	}
	deletePhoto(photo: Photo) {
		this.membersService.deletePhoto(photo).subscribe({
			next: (_) => {
				const updatedMember = { ...this.member() };
				updatedMember.photos = updatedMember.photos.filter(
					(x) => x.id !== photo.id
				);
				this.memberChange.emit(updatedMember);
			},
		});
	}
}
