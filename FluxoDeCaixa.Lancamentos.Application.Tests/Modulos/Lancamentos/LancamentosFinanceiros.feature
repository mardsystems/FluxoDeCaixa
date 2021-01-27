#language: pt-br
Funcionalidade: Lançamentos Financeiros
	Para gerenciar minhas finanças
	Enquanto Marcelo
	Eu quero *lançar* pagamentos e recebimentos

#Contexto: Estou na minha conta do Nubank

Cenário: Marcelo Lança um Pagamento com Sucesso
	Dado que estou na minha conta (banco: 001, conta Corrente: 567, cpf: 096) com saldo positivo de 123,45 reais
	Quando lanço um pagamento de 50 reais nela sob o protocolo 123456789
	Então o saldo da conta deve ser de 73,45 reais
	E um lançamento no valor de 50 reais deve ser adicionado sob o número de protocolo 123456789

#Esquema do Cenario: xxx
#
#	Exemplos: xxxo
#		| x | y |
#		| 1 | 2 |