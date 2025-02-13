import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { MemberCardComponent } from '../member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../_services/account.service';
import { UserParams } from '../../_models/userParams';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
@Component({
	selector: 'app-members-list',
	standalone: true,
	imports: [
		MemberCardComponent,
		PaginationModule,
		FormsModule,
		ButtonsModule,
	],
	templateUrl: './members-list.component.html',
	styleUrl: './members-list.component.css',
})
export class MembersListComponent implements OnInit {
	membersService = inject(MembersService);
	genderList = [
		{ value: 'male', display: 'Male' },
		{ value: 'female', display: 'Female' },
	];
	ngOnInit(): void {
		if (!this.membersService.paginatedResult()) this.loadMembers();
	}
	loadMembers() {
		this.membersService.getMembers();
	}
	resetFilters() {
		this.membersService.resetUserParams();
		this.loadMembers();
	}
	pageChanged(event: any) {
		if (this.membersService.userParams().pageNumber !== event.page) {
			this.membersService.userParams().pageNumber = event.page;
			this.loadMembers();
		}
	}
}
