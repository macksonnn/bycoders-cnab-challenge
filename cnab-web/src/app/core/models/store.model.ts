import { Transaction } from './transaction.model';

export interface Store {
    id: number;
    name: string;
    owner: string;
    balance: number;
    transactions: Transaction[];
}

