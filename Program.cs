using System;
using System.Collections;
using System.Formats.Asn1;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Runtime.Intrinsics.Arm;
class RegistroDeVoo
{
    public int Numero;
    public string Origem;
    public string Destino;
    public int NumVagasOriginal;
    public int NumVagas;
    public List<RegistroDePassagem> ListaPassageiros;
    public Queue<RegistroDePassagem> FilaPassageiros = new Queue<RegistroDePassagem>();
    public RegistroDeVoo()
    {
        ListaPassageiros = new List<RegistroDePassagem>(NumVagas);
    }
}
class RegistroDePassagem
{
    public string CPF;
    public int NumVoo;
    public string DataVoo;
}
class Program
{
    static void AdicionarPassageiro(List<RegistroDeVoo> ListaVoos) // Lista voos e adiciona passageiros
    {
        bool Encontrou = false;

        do // Loop para escolher o voo
        {
            ListarVoos(ListaVoos);

            Console.Write("\nNumero do voo : ");
            int NumVoo = int.Parse(Console.ReadLine());

            // Animação de loading
            for (int i = 0; i < 3; i++)
            {
                Console.Write(".");
                Thread.Sleep(400);
            }

            Console.WriteLine("\n");

            // Procura o numero do voo na lista
            foreach (RegistroDeVoo voo in ListaVoos)
                if (voo.Numero == NumVoo) // Se o voo existe
                {
                    Encontrou = true;
                    Console.WriteLine("Voo encontrado!");
                    Thread.Sleep(500);

                    do // Loop adicão de passageiro na lista/fila
                    {
                        Console.Clear();

                        Console.WriteLine($"\nVoo - {voo.Numero}");
                        Console.WriteLine($"De : {voo.Origem}  Para : {voo.Destino}");
                        Console.WriteLine($"Passageiros : {voo.ListaPassageiros.Count()}");
                        Console.WriteLine($"Nº Vagas - {voo.NumVagas}");

                        if (voo.NumVagas == 0 && voo.FilaPassageiros.Count() > 0)
                            Console.WriteLine($"Há fila - {voo.FilaPassageiros.Count()} Passageiro(s)");

                        if (voo.NumVagas > 0) // Se houver vagas no voo
                        {
                            RegistroDePassagem Passageiro = new RegistroDePassagem();// Cria novo passageiro

                            Console.WriteLine("\nAdicionando na lista de passageiros.");

                            Console.WriteLine("Informe o CPF do passageiro.");
                            Passageiro.CPF = Console.ReadLine();
                            Console.WriteLine("Informe a data do voo");
                            Passageiro.DataVoo = Console.ReadLine();
                            Passageiro.NumVoo = voo.Numero;

                            voo.ListaPassageiros.Add(Passageiro);
                            voo.NumVagas--;

                            Console.WriteLine("Passageiro adicionado.\n");

                            Console.WriteLine("Deseja adicionar mais passageiros?(Esc para cancelar)");
                        }
                        else // Não há vagas
                        {
                            string escolha;
                            bool Adicionar = false; // Controla o loop de adiçao de passageiros na fila

                            Console.WriteLine("Não há mais vagas nesse voo.");

                            do // Loop para respostas invalidas no cadastro de fila
                            {
                                Console.WriteLine("\nCadastrar passageiro na fila ?(S/N)");
                                Console.WriteLine($"Há {voo.FilaPassageiros.Count()} passageiro(s) na fila.\n");

                                escolha = Console.ReadLine().ToUpper(); // Converte para maiuscula

                                if (escolha == "S")
                                {
                                    Adicionar = true;

                                    do // Loop que adiciona passageiros na fila
                                    {
                                        RegistroDePassagem Passageiro = new RegistroDePassagem();// Cria novo passageiro

                                        Console.Clear();

                                        Console.WriteLine($"\n{voo.FilaPassageiros.Count()} passageiro(s) na fila\n");

                                        Console.WriteLine("Informe o CPF do passageiro.");
                                        Passageiro.CPF = Console.ReadLine();
                                        Console.WriteLine("Informe a data do voo");
                                        Passageiro.DataVoo = Console.ReadLine();
                                        Passageiro.NumVoo = voo.Numero;

                                        voo.FilaPassageiros.Enqueue(Passageiro); // Adiciona o Passageiro na Fila de passageiros de seu respectivo Voo

                                        Console.WriteLine("\nPassageiro adicionado na fila.");
                                        Console.WriteLine("Adicionar mais passageiros na fila?(Esc para cancelar)\n");

                                        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                                        {
                                            Adicionar = false;
                                            break;
                                        }

                                    } while (Adicionar == true);

                                    break;
                                }
                                else if (escolha != "N")
                                {
                                    Console.WriteLine("Opção invalida.\n");
                                    break;
                                }

                            } while (escolha != "N");

                            if (escolha == "N" || Adicionar || escolha == "S")
                                break; // Impede adição de mais passageiros
                        }
                    }
                    while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                    break; // Impede a busca por outro voo
                }

            if (!Encontrou)// Voo não encontrado
            {
                Console.WriteLine($"Voo {NumVoo} não encontrado!");
                Console.WriteLine("Deseja digitar o numero novamente?(S/N)");

                string escolha = Console.ReadLine().ToUpper();

                if (escolha == "N")
                    Encontrou = true; // Impede a busca por voo

                else if (escolha != "S")
                    Console.WriteLine("Resposta invalida");

                Console.Clear();
            }

        } while (!Encontrou);
    }
    static void AdicionarPassageiro(List<RegistroDeVoo> ListaVoos, int NumVoo) // Adiciona passageiros em voo especifico
    {
        foreach (RegistroDeVoo voo in ListaVoos)
            if (voo.Numero == NumVoo)
            {
                bool Adicionar = false; // Controla o loop de adiçao de passageiros na fila

                do // Loop adicão de passageiro na lista/fila
                {
                    RegistroDePassagem Passageiro = new RegistroDePassagem();// Cria novo passageiro

                    Console.Clear();

                    Console.WriteLine($"\nVoo - {voo.Numero}");
                    Console.WriteLine($"De : {voo.Origem}  Para : {voo.Destino}");
                    Console.WriteLine($"Passageiros : {voo.ListaPassageiros.Count()}");
                    Console.WriteLine($"Nº Vagas - {voo.NumVagas}");

                    if (voo.NumVagas == 0 && voo.FilaPassageiros.Count() > 0)
                        Console.WriteLine($"Há fila - {voo.FilaPassageiros.Count()} Passageiro(s)");

                    if (voo.NumVagas > 0) // Se houver vagas no voo
                    {
                        Console.WriteLine("\nAdicionando na lista de passageiros.");

                        Console.WriteLine("Informe o CPF do passageiro.");
                        Passageiro.CPF = Console.ReadLine();
                        Console.WriteLine("Informe a data do voo");
                        Passageiro.DataVoo = Console.ReadLine();
                        Passageiro.NumVoo = voo.Numero;

                        voo.ListaPassageiros.Add(Passageiro);
                        voo.NumVagas--;

                        Console.WriteLine("Passageiro adicionado.\n");

                        Console.WriteLine("Deseja adicionar mais passageiros?(Esc para cancelar)");

                        if (Console.ReadKey(true).Key != ConsoleKey.Escape) // Não apertou ESC o loop para adicionar repete
                            Adicionar = true;
                    }
                    else // Não há vagas
                    {
                        Adicionar = false;
                        bool Adicionarfila = false;
                        string escolha;

                        Console.WriteLine("Não há mais vagas nesse voo.");

                        do // Loop para respostas invalidas no cadastro de fila
                        {
                            Console.WriteLine("\nCadastrar passageiro na fila ?(S/N)");
                            Console.WriteLine($"Há {voo.FilaPassageiros.Count()} passageiro(s) na fila.\n");

                            escolha = Console.ReadLine().ToUpper(); // Converte para maiuscula

                            if (escolha == "S")
                            {
                                Adicionarfila = true;

                                do // Loop que adiciona passageiros na fila
                                {
                                    Console.Clear();

                                    Console.WriteLine($"\n{voo.FilaPassageiros.Count()} passageiro(s) na fila\n");

                                    Console.WriteLine("Informe o CPF do passageiro.");
                                    Passageiro.CPF = Console.ReadLine();
                                    Console.WriteLine("Informe a data do voo");
                                    Passageiro.DataVoo = Console.ReadLine();
                                    Passageiro.NumVoo = voo.Numero;

                                    voo.FilaPassageiros.Enqueue(Passageiro); // Adiciona o Passageiro na Fila de passageiros de seu respectivo Voo

                                    Console.WriteLine("\nPassageiro adicionado na fila.");
                                    Console.WriteLine("Adicionar mais passageiros na fila?(Esc para cancelar)\n");

                                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                                    {
                                        Adicionar = false;
                                        break;
                                    }

                                } while (Adicionarfila == true);

                                break;
                            }
                            else if (escolha != "N")
                            {
                                Console.WriteLine("Opção invalida.\n");
                                break;
                            }

                        } while (escolha != "N");
                    }
                }
                while (Adicionar);

                break; // Break para impedir a busca por outro voo
            }
    }
    static int Menu() // Exibe menu
    {
        int opçao;

        do
        {
            Console.Clear();

            Console.WriteLine("===========================");
            Console.WriteLine("     Compahia de Voos");
            Console.WriteLine("===========================");
            Console.WriteLine(" 1 - Adicionar voo");
            Console.WriteLine(" 2 - Adicionar passageiro");
            Console.WriteLine(" 3 - Listar voos");
            Console.WriteLine(" 4 - Listar passageiros");
            Console.WriteLine(" 5 - Remover voo");
            Console.WriteLine(" 6 - Remover passageiro");
            Console.WriteLine(" 7 - Sair");
            Console.WriteLine("===========================\n");

            opçao = int.Parse(Console.ReadLine());

        } while (opçao > 7 && opçao < 1);

        return opçao;
    }
    static void ListarVoos(List<RegistroDeVoo> ListaVoos) // Lista todos os voos
    {
        if (ListaVoos.Count() > 0)  // Há voos
        {
            if (ListaVoos.Count() == 1) // 1 voo registrado
                Console.WriteLine("\nHá apenas um voo cadastrado:");

            else if (ListaVoos.Count() > 1) // Mais de 1 voo
                Console.WriteLine($"\nVoos cadastrados {{{ListaVoos.Count()}}}:");

            foreach (RegistroDeVoo Voo in ListaVoos)
            {
                Console.WriteLine($"\nVoo - {Voo.Numero}");
                Console.WriteLine($"Origem : {Voo.Origem}  Destino : {Voo.Destino}");
                Console.WriteLine($"Nº Vagas - {Voo.NumVagas}");

                if (Voo.FilaPassageiros.Count() > 0)
                    Console.WriteLine($"Há fila - {Voo.FilaPassageiros.Count()} Passageiro(s)");
                else
                    Console.WriteLine("Não há fila.");
            }
        }
        else  // 0 voos registrados
            Console.WriteLine("Não há voos cadastrados.\n");

    }
    static void ListarPassageiros(List<RegistroDeVoo> ListaVoos) // Lista os voos com passageiros
    {
        if (ListaVoos.Count() > 0)
            foreach (RegistroDeVoo Voo in ListaVoos) // Lista todos voos na lista 
            {
                Console.WriteLine($"\n Voo - {Voo.Numero}");
                Console.WriteLine("--------------------");
                Console.WriteLine($"Origem : {Voo.Origem}  -  Destino : {Voo.Destino}");
                Console.WriteLine($"Nº Passageiros - {Voo.ListaPassageiros.Count}");
                Console.WriteLine($"Nº Vagas - {Voo.NumVagas}");

                if (Voo.ListaPassageiros.Count() > 0)// Verifica a existencia de passageiros na lista de voos
                {
                    Console.WriteLine("Lista de passageiros :");

                    // Listagem de passageiros da lista de voo
                    foreach (RegistroDePassagem Passageiro in Voo.ListaPassageiros)
                        Console.WriteLine($"  - CPF : {Passageiro.CPF}  -  Voo em {Passageiro.DataVoo}");

                    // Verifica a existencia de passageiros na fila de espera
                    if (Voo.FilaPassageiros.Count() > 0)
                    {
                        Console.WriteLine("\nFila de espera :");

                        foreach (RegistroDePassagem Passageiro in Voo.FilaPassageiros) // Lista passageiros da fila
                            Console.WriteLine($"  - CPF : {Passageiro.CPF}  -  Voo em {Passageiro.DataVoo}");
                    }
                }
            }
        else
            Console.WriteLine("Não há voos cadastrados.");
    }
    static void ListarPassageiros(List<RegistroDeVoo> ListaVoos, int NumVoo) // Lista um voo e seus passageiros
    {
        foreach (RegistroDeVoo Voo in ListaVoos) // Lista todos voos na lista 
            if (Voo.Numero == NumVoo)
            {
                Console.WriteLine($"\n Voo - {Voo.Numero}");
                Console.WriteLine("--------------------");
                Console.WriteLine($"Origem : {Voo.Origem}  -  Destino : {Voo.Destino}");
                Console.WriteLine($"Nº passageiros - {Voo.ListaPassageiros.Count}");
                Console.WriteLine($"Nº Vagas - {Voo.NumVagas}");

                // Listagem de passageiros
                if (Voo.ListaPassageiros.Count() > 0)// Verifica a existencia de passageiros na lista de voos
                {
                    Console.WriteLine("Lista de passageiros :");

                    // Listagem de passageiros da lista de voo
                    foreach (RegistroDePassagem Passageiro in Voo.ListaPassageiros)
                        Console.WriteLine($"  - CPF : {Passageiro.CPF}  -  Voo em {Passageiro.DataVoo}");

                    // Verifica a existencia de passageiros na fila de espera
                    if (Voo.FilaPassageiros.Count() > 0)
                    {
                        Console.WriteLine("\nFila de espera :");

                        foreach (RegistroDePassagem Passageiro in Voo.ListaPassageiros) // Lista passageiros da fila
                            Console.WriteLine($"  - CPF : {Passageiro.CPF}  -  Voo em {Passageiro.DataVoo}");
                    }
                }
            }
    }
    static void Main(string[] args)
    {
        List<RegistroDeVoo> ListaVoos = new List<RegistroDeVoo>();
        int opçao;

        do
        {
            opçao = Menu();

            switch (opçao)
            {
                case 1: // Adiciona voos (Perfeito)

                    do // Loop que adiciona voos
                    {
                        Console.Clear();

                        RegistroDeVoo Voo = new RegistroDeVoo();
                        bool Registrado = false;

                        Console.WriteLine("\nInforme o numero do voo");
                        Voo.Numero = int.Parse(Console.ReadLine());

                        //Verifica se ja existe algum voo com esse numero
                        foreach (RegistroDeVoo vooregistrado in ListaVoos)
                            if (Voo.Numero == vooregistrado.Numero)
                                Registrado = true;

                        if (!Registrado) // Voo com numero novo 
                        {
                            // Continua o registro
                            Console.Write("Origem  : ");
                            Voo.Origem = Console.ReadLine();
                            Console.Write("Destino : ");
                            Voo.Destino = Console.ReadLine();
                            Console.Write("Numero de vagas : ");
                            Voo.NumVagas = int.Parse(Console.ReadLine());

                            Voo.NumVagasOriginal = Voo.NumVagas;

                            ListaVoos.Add(Voo);

                            Console.WriteLine("\nVoo adicionado.");
                            Console.WriteLine("Adicionar mais voos? (Esc para cancelar)\n");
                        }
                        else // Numero ja existente
                        {
                            Console.WriteLine("\nNumero de voo já registrado");
                            Console.WriteLine("Tentar novamente? (Esc para cancelar)\n");
                        }

                    } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                    break;
                case 2: // Adiciona passageiros (Perfeito)

                    if (ListaVoos.Count() == 0) // Não há voos
                    {
                        Console.WriteLine("Não há voos registrados...");
                        Console.ReadKey(true);
                    }
                    else if (ListaVoos.Count() == 1) // Existe apenas um voo
                    {
                        string Escolha;
                        do
                        {
                            int NumVoo = 0;

                            Console.Clear();
                            Console.WriteLine("\nHá apenas um voo disponivel : ");

                            foreach (RegistroDeVoo voo in ListaVoos)
                            {
                                Console.WriteLine($"\nVoo - {voo.Numero}");
                                Console.WriteLine($"De : {voo.Origem}  Para : {voo.Destino}");
                                Console.WriteLine($"Passageiros : {voo.ListaPassageiros.Count()}");
                                Console.WriteLine($"Nº Vagas - {voo.NumVagas}");

                                NumVoo = voo.Numero;

                                if (voo.NumVagas == 0 && voo.FilaPassageiros.Count() > 0)
                                    Console.WriteLine($"Há fila - {voo.FilaPassageiros.Count()} Passageiro(s)");
                            }
                            Console.WriteLine("\nAdicionar passageiro nesse voo? (S/N)");
                            Escolha = Console.ReadLine().ToUpper();

                            if (Escolha == "S")
                                AdicionarPassageiro(ListaVoos, NumVoo);

                            else if (Escolha != "N")
                            {
                                Console.WriteLine("Escolha inválida");
                                Console.ReadKey(true);
                            }

                        } while (Escolha != "S" && Escolha != "N");
                    }
                    else if (ListaVoos.Count() > 1) // Mais de um voo, Exibe opções
                    {
                        int escolha;
                        do // Loop para a escolha
                        {
                            Console.Clear();

                            Console.WriteLine("-------------------------");
                            Console.WriteLine("  Adiçao de passageiros");
                            Console.WriteLine("-------------------------");
                            Console.WriteLine(" 1 - Ver todos os voos.");
                            Console.WriteLine(" 2 - Adicionar em voo especifico.");
                            Console.WriteLine(" 3 - Voltar.\n");
                            escolha = int.Parse(Console.ReadLine());

                            if (escolha == 1) // Mostra os voo depois Adiciona o passageiro
                            {
                                Console.Clear();

                                AdicionarPassageiro(ListaVoos);
                            }
                            else if (escolha == 2) // Adiciona no voo
                            {
                                bool existe = false;

                                Console.Write("\nNumero do voo : ");
                                int NumVoo = int.Parse(Console.ReadLine());

                                foreach (RegistroDeVoo Voo in ListaVoos) // Verifica a existência do voo 
                                    if (Voo.Numero == NumVoo)
                                        existe = true;

                                if (existe)
                                {
                                    Console.Clear();

                                    AdicionarPassageiro(ListaVoos, NumVoo);
                                }
                                else
                                {
                                    Console.WriteLine($"Esse voo não existe....");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 3) // Retorna a tela principal
                            {
                                for (int i = 0; i < 4; i++) // Animação de loading
                                {
                                    Console.Write(".");
                                    Thread.Sleep(400);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Opção invalida");
                                Console.ReadKey(true);
                            }

                        } while (escolha != 3);
                    }
                    break;
                case 3: // Lista voos (Perfeito)

                    if (ListaVoos.Count() == 0) // Não há voos
                    {
                        Console.WriteLine("Não há voos registrados...");
                        Console.ReadKey(true);
                    }
                    if (ListaVoos.Count() > 0) // Há voos
                    {
                        Console.Clear();

                        ListarVoos(ListaVoos);

                        Console.ReadKey(true);
                    }
                    break;
                case 4: // Lista passageiros (Perfeito)

                    if (ListaVoos.Count() == 0) // Não há voos
                    {
                        Console.WriteLine("Não há voos registrados...");
                        Console.ReadKey(true);
                    }
                    else if (ListaVoos.Count() == 1) // Existe apenas um voo
                    {
                        Console.Clear();

                        foreach (RegistroDeVoo Voo in ListaVoos)
                            Console.WriteLine($"\nHá apenas 1 voo registrado com {{{Voo.ListaPassageiros.Count()}}} passageiros");

                        ListarPassageiros(ListaVoos);

                        Console.ReadKey(true);
                    }
                    else if (ListaVoos.Count() > 1) // Mais de um voo, Exibe opções
                    {
                        int ContVoos = 0, escolha;

                        // Conta os voos com passageiros
                        foreach (RegistroDeVoo Voo in ListaVoos)
                            if (Voo.ListaPassageiros.Count() > 0)
                                ContVoos++;

                        do // Loop para a Escolha
                        {
                            Console.Clear();

                            Console.WriteLine($"\nHá {ListaVoos.Count()} voo(s) registrado(s)");
                            Console.WriteLine($"Há {ContVoos} voo(s) com passageiro(s).\n");

                            Console.WriteLine(" Listagem de passageiros");
                            Console.WriteLine("-------------------------");
                            Console.WriteLine(" 1 - Passageiros em todos voos.");
                            Console.WriteLine(" 2 - Passageiros em voo especifico.");
                            Console.WriteLine(" 3 - Voltar.\n");
                            escolha = int.Parse(Console.ReadLine());

                            if (escolha == 1) // Lista todos os voos
                            {
                                Console.Clear();

                                ListarPassageiros(ListaVoos);

                                Console.ReadKey(true);
                            }
                            else if (escolha == 2) // Lista o voo do numero "NumVoo"
                            {
                                bool existe = false;

                                Console.Write("\nNumero do voo : ");
                                int NumVoo = int.Parse(Console.ReadLine());

                                foreach (RegistroDeVoo Voo in ListaVoos) // Verifica a existência do voo 
                                    if (Voo.Numero == NumVoo)
                                        existe = true;

                                if (existe)
                                {
                                    Console.Clear();

                                    ListarPassageiros(ListaVoos, NumVoo);
                                }
                                else
                                {
                                    Console.WriteLine($"Esse voo não existe....");
                                    Console.ReadKey(true);
                                }

                                Console.ReadKey(true);
                            }
                            else if (escolha == 3)
                            {
                                // Animação de loading
                                for (int i = 0; i < 4; i++)
                                {
                                    Console.Write(".");
                                    Thread.Sleep(300);
                                }

                                break;
                            }
                            else
                            {
                                Console.WriteLine("Opção invalida");
                                Console.ReadKey(true);
                            }

                        } while (escolha != 3);
                    }
                    break;
                case 5: // Remover voo (PERFEITO)

                    if (ListaVoos.Count() == 0) // Não há voos
                    {
                        Console.WriteLine("Não há voos registrados...");
                        Console.ReadKey(true);
                    }
                    else if (ListaVoos.Count() == 1) // Existe apenas um voo
                    {
                        Console.Clear();

                        ListarVoos(ListaVoos);

                        Console.WriteLine("\nApaga-lo? (Enter confirma)");

                        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                        {
                            ListaVoos.Clear();

                            Console.WriteLine("Voo apagado.");
                        }
                        else
                            Console.WriteLine("\nExclusão cancelada.");

                        Console.ReadKey(true);
                    }
                    else if (ListaVoos.Count() > 1) // Mais de um voo, Exibe opções
                    {
                        int escolha;
                        do // Loop para a escolha
                        {
                            Console.Clear();

                            Console.WriteLine("---------------------");
                            Console.WriteLine("  Exclusão de voos");
                            Console.WriteLine("--------------------");
                            Console.WriteLine(" 1 - Excluir todos os voos.");
                            Console.WriteLine(" 2 - Excluir voo especifico.");
                            Console.WriteLine(" 3 - Voltar.\n");
                            escolha = int.Parse(Console.ReadLine());

                            if (escolha == 1) // Exibe todos para a exclusão
                            {
                                Console.Clear();

                                ListarVoos(ListaVoos);

                                Console.WriteLine("\nEsses são os voos registrados.");
                                Console.WriteLine("Apaga-los? (Enter confirma)");

                                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                {
                                    ListaVoos.Clear();

                                    Console.WriteLine("Voos apagados.");
                                    break;
                                }
                                else
                                    Console.WriteLine("\nExclusão cancelada.");

                                Console.ReadKey(true);
                            }
                            else if (escolha == 2) // Remove voo por numero
                            {
                                bool Existe = false;

                                Console.Clear();

                                Console.Write("\nNumero do voo : ");
                                int NumVoo = int.Parse(Console.ReadLine());

                                foreach (RegistroDeVoo Voo in ListaVoos) // Verifica a existência do voo 
                                    if (Voo.Numero == NumVoo) // Voo existe
                                    {
                                        Existe = true;

                                        Console.WriteLine($"\nVoo - {Voo.Numero}");
                                        Console.WriteLine($"Origem : {Voo.Origem}  Destino : {Voo.Destino}");
                                        Console.WriteLine($"Nº Vagas - {Voo.NumVagas}");

                                        if (Voo.FilaPassageiros.Count() > 0)
                                            Console.WriteLine($"Há fila - {Voo.FilaPassageiros.Count()} Passageiro(s)");
                                        else
                                            Console.WriteLine("Não há fila.");

                                        Console.WriteLine("\nApaga-lo? (Enter confirma)");

                                        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                        {
                                            ListaVoos.Remove(Voo);

                                            Console.WriteLine("Voo apagado.");
                                        }
                                        else
                                            Console.WriteLine("\nExclusão cancelada");

                                        Console.ReadKey(true);

                                        break; // Impede a busca por novo voos
                                    }

                                if (!Existe) // Voo inexistente
                                {
                                    Console.WriteLine("Esse voo não existe.");
                                    Console.ReadKey(true);
                                }
                            }
                            else if (escolha == 3) // Retorna a tela principal
                            {
                                for (int i = 0; i < 4; i++) // Animação de loading
                                {
                                    Console.Write(".");
                                    Thread.Sleep(400);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Opção invalida");
                                Console.ReadKey(true);
                            }

                        } while (escolha != 3);
                    }
                    break;
                case 6: // Remover passageiro (Perfeito)

                    if (ListaVoos.Count() == 0) // Não há voos
                    {
                        Console.WriteLine("Não há voos registrados...");
                        Console.ReadKey(true);
                    }
                    else if (ListaVoos.Count() == 1) // Existe apenas um voo
                    {
                        foreach (RegistroDeVoo Voo in ListaVoos)
                            if (Voo.ListaPassageiros.Count() > 0)
                            {
                                int escolha;
                                bool Repetir = false; // bool para repetir em caso de CPF invalido
                                do
                                {
                                    Repetir = false;

                                    Console.Clear();

                                    Console.WriteLine($"\nHá apenas 1 voo registrado com {{{Voo.ListaPassageiros.Count()}}} passageiros");

                                    ListarPassageiros(ListaVoos);

                                    Console.WriteLine("\n---------------------------");
                                    Console.WriteLine("  Exclusão de passageiros");
                                    Console.WriteLine("---------------------------");
                                    Console.WriteLine(" 1 - Excluir todos os passageiros.");
                                    Console.WriteLine(" 2 - Excluir passageiro especifico.");
                                    Console.WriteLine(" 3 - Voltar.\n");
                                    escolha = int.Parse(Console.ReadLine());

                                    if (escolha == 1) // Remove todos passageiros
                                    {
                                        Voo.NumVagas = Voo.NumVagasOriginal; // Numero de vagas resetado

                                        Voo.ListaPassageiros.Clear();
                                        Voo.FilaPassageiros.Clear();

                                        Console.WriteLine("Passageiros apagados.");

                                        Console.ReadKey(true);
                                    }
                                    else if (escolha == 2) // Remove passageiro por CPF
                                    {
                                        bool ExistePassageiro = false;

                                        Console.Write("\nCPF para exclusão da lista : ");
                                        string CPF = Console.ReadLine();

                                        foreach (RegistroDePassagem Passageiro in Voo.ListaPassageiros) // Verifica a existência do passageiro na LISTA
                                            if (CPF == Passageiro.CPF)// Passageiro existe na LISTA
                                            {
                                                ExistePassageiro = true;

                                                Console.Clear();

                                                Console.WriteLine($"\nPassageiro: \nCPF - {Passageiro.CPF}\nData do voo - {Passageiro.DataVoo}");

                                                Console.WriteLine("\nApaga-lo? (Enter confirma)");

                                                if (Console.ReadKey(true).Key == ConsoleKey.Enter) // Remove passageiro da lista
                                                {
                                                    // Remove o passageiro do index

                                                    Voo.ListaPassageiros.Remove(Passageiro);
                                                    Voo.NumVagas++;

                                                    Console.WriteLine("\nPassageiro apagado.");

                                                    if (Voo.FilaPassageiros.Count() > 0) // Adiciona o primeiro da fila na lista
                                                    {
                                                        RegistroDePassagem PrimeiroFila;

                                                        PrimeiroFila = Voo.FilaPassageiros.Dequeue();

                                                        Voo.ListaPassageiros.Add(PrimeiroFila);
                                                        Voo.NumVagas--;
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("\nExclusão cancelada");
                                                    Console.ReadKey(true);
                                                }

                                                break; // Evita busca desnecessária
                                            }

                                        if (!ExistePassageiro)
                                        {
                                            Console.WriteLine("Passageiro inexistente.");
                                            Repetir = true;
                                            Console.ReadKey(true);
                                        }

                                        Console.ReadKey(true);
                                    }
                                    else if (escolha == 3) // Retorna a tela principal
                                    {
                                        for (int i = 0; i < 4; i++) // Animação de loading
                                        {
                                            Console.Write(".");
                                            Thread.Sleep(400);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Opção invalida");
                                        Console.ReadKey(true);
                                    }

                                } while (Repetir || escolha != 3);
                            }
                            else
                            {
                                Console.WriteLine("Não há passageiros registrados.");
                                Console.ReadKey(true);
                            }
                    }
                    else if (ListaVoos.Count() > 1) // há voos
                    {
                        bool HaPassageiros = false;

                        foreach (RegistroDeVoo VOO in ListaVoos)
                            if (VOO.ListaPassageiros.Count() > 0)
                            {
                                int escolha;
                                HaPassageiros = true;

                                do // Loop para a escolha
                                {
                                    Console.Clear();

                                    Console.WriteLine("---------------------------");
                                    Console.WriteLine("  Exclusão de passageiros");
                                    Console.WriteLine("---------------------------");
                                    Console.WriteLine(" 1 - Excluir todos os passageiros.");
                                    Console.WriteLine(" 2 - Excluir passageiros de um voo.");
                                    Console.WriteLine(" 3 - Excluir passageiro especifico.");
                                    Console.WriteLine(" 4 - Voltar.\n");
                                    escolha = int.Parse(Console.ReadLine());

                                    if (escolha == 1) // Remove todos passageiros
                                    {
                                        Console.Clear();

                                        ListarPassageiros(ListaVoos);

                                        Console.WriteLine("\nEsses são os voos registrados.");
                                        Console.WriteLine("Apaga-los? (Enter confirma)");

                                        if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                        {
                                            foreach (RegistroDeVoo Voo in ListaVoos)
                                            {
                                                Voo.NumVagas = Voo.NumVagasOriginal; // Numero de vagas resetado

                                                Voo.ListaPassageiros.Clear();
                                                Voo.FilaPassageiros.Clear();
                                            }

                                            Console.WriteLine("Passageiros apagados.");
                                        }
                                        else
                                            Console.WriteLine("\nExclusão cancelada");

                                        Console.ReadKey(true);
                                    }
                                    else if (escolha == 2) // Remove passageiros de um voo
                                    {
                                        bool Existe = false;

                                        Console.Write("\nNumero do voo : ");
                                        int NumVoo = int.Parse(Console.ReadLine());

                                        foreach (RegistroDeVoo Voo in ListaVoos) // Verifica a existência do voo 
                                            if (Voo.Numero == NumVoo) // Voo existe
                                            {
                                                Existe = true;

                                                Console.Clear();

                                                ListarPassageiros(ListaVoos, NumVoo);

                                                Console.WriteLine("\nApaga-los? (Enter confirma)");

                                                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                                                {
                                                    Voo.NumVagas = Voo.NumVagasOriginal; // Reseta as vagas

                                                    Voo.ListaPassageiros.Clear();
                                                    Voo.FilaPassageiros.Clear();

                                                    Console.WriteLine("Passageiros apagados.");
                                                }
                                                else
                                                    Console.WriteLine("\nExclusão cancelada");

                                                Console.ReadKey(true);

                                                break; // Impede a busca por novo voos
                                            }
                                        if (!Existe) // Voo inexistente
                                        {
                                            Console.WriteLine("Esse voo não existe.");
                                            Console.ReadKey(true);
                                        }
                                    }
                                    else if (escolha == 3) // Remove passageiro por CPF
                                    {
                                        bool ExisteVoo = false;

                                        Console.Write("\nNumero do voo : ");
                                        int NumVoo = int.Parse(Console.ReadLine());

                                        foreach (RegistroDeVoo Voo in ListaVoos) // Verifica a existência do voo 
                                            if (Voo.Numero == NumVoo) // Voo existe
                                            {
                                                ExisteVoo = true;
                                                bool ExistePassageiro = false;
                                                bool RepetirCPF = false; // bool para repetir em caso de CPF invalido


                                                Console.Clear();

                                                ListarPassageiros(ListaVoos, NumVoo);

                                                Console.Write("\nCPF para exclusão da lista : ");
                                                string CPF = Console.ReadLine();

                                                foreach (RegistroDePassagem Passageiro in Voo.ListaPassageiros) // Verifica a existência do passageiro na LISTA
                                                    if (CPF == Passageiro.CPF)// Passageiro existe na LISTA
                                                    {
                                                        ExistePassageiro = true;

                                                        Console.Clear();

                                                        Console.WriteLine($"\nPassageiro: \nCPF - {Passageiro.CPF}\nData do voo - {Passageiro.DataVoo}");

                                                        Console.WriteLine("\nApaga-lo? (Enter confirma)");

                                                        if (Console.ReadKey(true).Key == ConsoleKey.Enter) // Remove passageiro da lista
                                                        {
                                                            // Remove o passageiro do index

                                                            Voo.ListaPassageiros.Remove(Passageiro);
                                                            Voo.NumVagas++;

                                                            Console.WriteLine("\nPassageiro apagado.");

                                                            if (Voo.FilaPassageiros.Count() > 0) // Adiciona o primeiro da fila na lista
                                                            {
                                                                RegistroDePassagem PrimeiroFila;

                                                                PrimeiroFila = Voo.FilaPassageiros.Dequeue();

                                                                Voo.ListaPassageiros.Add(PrimeiroFila);
                                                                Voo.NumVagas--;
                                                            }
                                                            Console.ReadKey(true);
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("\nExclusão cancelada");
                                                            Console.ReadKey(true);
                                                        }

                                                        break; // Evita busca desnecessária
                                                    }

                                                if (!ExistePassageiro)
                                                {
                                                    Console.WriteLine("Passageiro inexistente.");
                                                    RepetirCPF = true;
                                                    Console.ReadKey(true);
                                                }

                                            }

                                        if (!ExisteVoo)
                                        {
                                            Console.WriteLine("Numero de voo inexistênte.");
                                            Console.ReadKey(true);
                                        }
                                    }
                                    else if (escolha == 4) // Retorna a tela principal
                                    {
                                        for (int i = 0; i < 4; i++) // Animação de loading
                                        {
                                            Console.Write(".");
                                            Thread.Sleep(400);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Opção invalida");
                                        Console.ReadKey(true);
                                    }

                                } while (escolha != 4);

                                break; // Impede busca por outro voo
                            }

                        if (!HaPassageiros)
                        {
                            Console.WriteLine("Não há passageiros registrados.");
                            Console.ReadKey(true);
                        }
                    }
                    break;
                case 7: // Encerra o programa (Perfeito)

                    Console.Write("Encerrando programa");

                    for (int i = 0; i < 4; i++)
                    {
                        Console.Write(".");
                        Thread.Sleep(400);
                    }

                    break;
                    //Defalt desnecessario
            }
        } while (opçao != 7);
    }
}
