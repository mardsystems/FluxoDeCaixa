#language: pt-br
Funcionalidade: Lançamentos Financeiros
	Para que a **empresa de pagamentos** possa *gerenciar* minhas **contas**
	Enquanto **cliente**
	Eu preciso *realizar* **lançamentos financeiros** nas minhas **contas**

Contexto:
	Dado que existem os seguintes bancos:
		| Nome   | Código |
		| Nubank | 001    |
	E que eu (cpf: '096') tenho uma conta corrente 'Nubank' (número: 567) cadastrada

Cenário: Lançamento financeiro processado com sucesso
	Dado que a minha conta hoje tem saldo de R$ 123,45
	Quando lanço um pagamento de 'Conta de luz' de R$ 50,00 hoje na minha conta
	Então um protocolo deve ser gerado
	Dado que se passaram 5 segundos
	Quando eu consulto meu lançamento usando o protocolo gerado
	Então um lançamento 'Conta de luz' no valor de R$ 50,00 deve ser retornado
	E o saldo da minha conta deve ser de R$ 73,45

#Para me livrar dos bancos (não precisar ir nos bancos *gerenciar* minhas **contas**)
#Contexto: Estou na minha conta do Nubank
#	Dado que eu tenho uma conta 'Nubank' (banco: '001', conta Corrente: '567', cpf: '096')
Cenário: Empresa lança um pagamento com sucesso
	Dado que a minha conta hoje tem saldo de R$ 123,45
	Quando lanço um pagamento de 'Conta de luz' de R$ 50,00 hoje na minha conta sob o protocolo '12345'
	Então o saldo da minha conta deve ser de R$ 73,45
	E um lançamento 'Conta de luz' no valor de R$ 50,00 deve ser adicionado na minha conta com o protocolo '12345'