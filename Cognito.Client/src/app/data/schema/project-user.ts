import { User } from './user';

export class ProjectUser {
    user: User;
    userId: number;
    projectId?: number;
    pendingTasks?: number;
}
