# DesagioBurger


# DesafioGoodBurger

Inicialização: Precisa rodar o projeto Web e API.


Escolhi o modelo DDD para desenvolver porque é um desafio pequeno e simples, não tenho que me preocupar com isso e também acho que se encaixa bem nesse desafio com para separar bem as regras de negócio dele... 


Comecei a adotar DTOs como record, em prática nesse projeto não muda nada, inclusive se mudar para class não muda nada, mas é só a forma mais correta de trabalhar mesmo com os DTO`s que são vão transportar os dados. 


Adicionei um tratador de exceções middleware bem simples, só para não quebrar ou fugir de padrÕes e estourar na cara do client (front). 


Usei o banco SqLite, para o contexto funcionar é só rodar a migration.  


O que eu deixei em memória ? 

private static readonly IReadOnlyList<ItemCardapio> _itens =
[
    new("X-BURGER", "X Burger",     5.00m, TipoItem.Sanduiche),
    new("X-EGG",    "X Egg",        4.50m, TipoItem.Sanduiche),
    new("X-BACON",  "X Bacon",      7.00m, TipoItem.Sanduiche),
    new("BATATA",   "Batata Frita", 2.00m, TipoItem.Acompanhamento),
    new("REFRI",    "Refrigerante", 2.50m, TipoItem.Bebida),
];


Essa parte, porque achei mais simples mesmo para o desafio. 


Usei IA ? Usei para finalizar e acelerar algumas partes do frontend e para alguns testes que estavam repetitivos.

<h1>Fim</h1>

Esse foi o resultado final, caso tenham interesse podemos ir para uma conversa mais técnica e objetiva. Obrigado! 
