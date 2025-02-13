import { Photo } from './photo';

export interface Member {
	id: number;
	username: string;
	age: number;
	gender: string;
	photoUrl: string;
	knownAs: string;
	created: string;
	lastActive: string;
	introduction: string;
	interests: string;
	lookingFor: string;
	city: string;
	country: string;
	photos: Photo[];
}
