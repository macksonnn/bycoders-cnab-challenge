export interface Transaction {
    id: number;
    type: number;
    typeDescription: string;
    occurredAt: string; // ISO string
    value: number;
    signedValue: number; // valor com sinal aplicado
    cpf: string;
    card: string;
}

