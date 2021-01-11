#language: pt-br

Funcionalidade: Lançamentos Financeiros
	Para gerenciar minhas finanças
	Enquanto Marcelo
	Eu quero *lançar* pagamentos e recebimentos

Cenário: Marcelo Lança um Pagamento com Sucesso
	Dado que estou na minha conta do Nubank com saldo positivo de 15 reais
	Quando lanço um pagamento de 50 reais nela
	Então o saldo da conta deve ser decrescido do valor do pagamento
	E um lançamento deve ser adicionado