import { User } from './user';

export interface Task {
  id: number;
  title: string;
  description: string;
  created: Date;
  ownerUser: User;
  assignedToUser: User;
  completed: boolean;
}
