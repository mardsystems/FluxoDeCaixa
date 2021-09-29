#language: pt-br
Funcionalidade: Lançamentos Financeiros
	Para gerenciar meus pagamentos e recebimentos
	Enquanto empresa
	Eu quero *realizar* lançamentos financeiros

#Contexto: Estou na minha conta do Nubank

Cenário: Empresa Lança um Pagamento com Sucesso
	Dado que estou na minha conta (banco: 001, conta Corrente: 567, cpf: 096) com saldo positivo de 123,45 reais
	Quando lanço um pagamento de "Conta de luz" de 50 reais nela no dia de hoje sob o protocolo 123456789
	Então o saldo da conta no dia de hoje deve ser de 73,45 reais
	E um lançamento no valor de 50 reais deve ser adicionado sob o número de protocolo 123456789

#Esquema do Cenario: xxx
#
#	Exemplos: xxxo
#		| x | y |
#		| 1 | 2 |