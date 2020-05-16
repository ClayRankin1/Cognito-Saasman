import { User } from './user';
import { ProjectUser } from './project-user';

export interface Project {
    id?: number;
    fullName: string;
    nickname: string;
    projectNo: string;
    description: string;
    isBillable: boolean;
    ownerId: number;
    owner: User;
    proxyId: number | null;
    proxy: User;
    clientNo: string;
    archivedOn: string | null;
    domainId: number;
    users: ProjectUser[];
}
