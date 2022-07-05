Imports System
Imports System.Threading

Module Program
    Dim random As New Random()

    Dim dogs As String()
    Dim dogStats As Integer(,)

    Dim playerDeck As New List(Of Integer)
    Dim computerDeck As New List(Of Integer)

    Dim computer As Boolean = False

    Sub Main(args As String())
        Dim options(,) As String = { {"p", "Play Game"}, {"q", "Quit"} }
        While True
            Console.WriteLine("Select an option: ")
            For i As Integer = 0 To options.GetLength(0) -1
                Console.WriteLine($"[{options(i, 0).ToUpper()}] {options(i, 1)}")
            Next
            Dim input = Console.ReadKey()
            Console.WriteLine()
            Select input.KeyChar
                Case "p"
                    dogs = GetDogs()
                    dogStats = GenerateStats(dogs.Length)
                    SplitCards()
                    PlayGame()
                Case "q"
                    Console.WriteLine("Quitting...")
                    Exit While
                Case Else
                    Console.WriteLine("Invalid Option")
            End Select
        End While
    End Sub

    Function GetDogs() As String()
        Console.WriteLine("Enter the amount of cards you want")
        
        Dim amountOfCards As Integer = Convert.ToInt32(Console.ReadLine())

        Console.WriteLine()

        If(amountOfCards Mod 2 <> 0) 
            Console.WriteLine("The amount of cards must be even!")
            return GetDogs()
        Else If (amountOfCards > 30)
            Console.WriteLine("The amount of cards must be less than 30!")
            return GetDogs()
        Else If (amountOfCards < 4)
            Console.WriteLine("The amount of cards must be greater than 4!")
            return GetDogs()
        End If

        Console.WriteLine($"Loading {amountOfcards.ToString()} cards...")
        Console.WriteLine()

        Dim dogs As String()
        dogs = IO.File.ReadAllText("dogs.txt").Split(new String() {Environment.NewLine},
                                       StringSplitOptions.None)

        dogs = ShuffleDeck(dogs)

        Array.Resize(dogs, amountOfCards)
        Return dogs
    End Function

    Function GenerateStats(amountOfCards As Integer) As Integer(,)
        Dim stats(amountOfCards, 4) As Integer

        For i As Integer = 0 To amountOfCards
            ' random.Next(INC, EXC)
            stats(i, 0) = random.Next(1, 6)
            stats(i, 1) = random.Next(1, 101)
            stats(i, 2) = random.Next(1, 11)
            stats(i, 3) = random.Next(1, 11)
        Next
        Return stats
    End Function

    Sub SplitCards()
        For i As Integer = 0 To dogs.Length - 1
            If (i MOD 2) = 0
                playerDeck.Add(i)
            Else
                computerDeck.Add(i)
            End If
        Next
    End Sub

    Function ShuffleDeck(dogs As String()) As String()
        Dim length As Integer = dogs.Length

        For i As Integer = 0 To length - 1
            Dim newIndex  As Integer = i + random.Next(length - i)
            
            Dim temp As String = dogs(i)
            dogs(i) = dogs(newIndex)
            dogs(newIndex) = temp
        Next
        Return dogs
    End Function

    Function GetCard(index As Integer) As Integer()
        Dim stats(4) As Integer
        For i As Integer = 0 To 3
            stats(i) = dogStats(index, i)
        Next
        Return stats
    End Function

    Sub PrintCard(index As Integer)
        Dim stats() As Integer = GetCard(index)
        Dim card As String = dogs(index)
        Console.WriteLine($"Name: {card}")
        Console.WriteLine($"Exercise: {stats(0)}/5")
        Console.WriteLine($"Intelligence: {stats(1)}/100")
        Console.WriteLine($"Friendliness: {stats(2)}/10")
        Console.WriteLine($"Drool: {stats(3)}/10")
    End Sub

    
    Sub PlayerWon()
        Console.WriteLine()
        Console.WriteLine("You won this round")
    End Sub

    Function PlayRound(Optional firstTime As Boolean = True)
        Console.WriteLine("------------------------")
        Dim categories() As String = { "exercise", "intelligence", "friendliness", "drool" }
        Dim optionsList As String = Join(categories, ", ")
        
        Dim playerIndex As Integer = playerDeck(0) 
        Dim computerIndex As Integer = computerDeck(0)

        Dim playerCard As Integer() = GetCard(playerIndex)
        Dim computerCard As Integer() = GetCard(computerIndex)

        If firstTime = True
            Console.WriteLine()
            Console.WriteLine("Your first card is: ")
            PrintCard(playerIndex)
        End If 

        Dim choice As String

        ' Get Choice
        If (computer = False)
                Console.WriteLine()
                Console.WriteLine($"Pick an option from: {optionsList}")
            choice = Console.ReadLine().ToLower()
            If(NOT categories.Contains(choice))
                Console.WriteLine()
                Console.WriteLine("Invalid Option")
                Return PlayRound(False)
            End If
        Else 
            Thread.Sleep(2000)
            choice = categories(random.Next(0, categories.Length))
            Console.WriteLine()
            Console.WriteLine($"I choose {choice}")
            Thread.Sleep(1500)
        End If
        
        Console.WriteLine()
        Console.WriteLine("My first card was: ")
        printCard(computerIndex)
        Thread.Sleep(2250)
        
        ' If player won
        Select choice
            Case "exercise"
                If playerCard(0) >= computerCard(0)
                    PlayerWon()
                    Return True
                End If
            Case "intelligence"
                If playerCard(1) >= computerCard(1)
                    PlayerWon()
                    Return True
                End If
            Case "friendliness"
                If playerCard(2) >= computerCard(2)
                    PlayerWon()
                    Return True
                End If
            Case "drool"
                If playerCard(3) <= computerCard(3)
                    PlayerWon()
                    Return True
                End If
        End Select
        ' If player lost
        Console.WriteLine()
        Console.WriteLine("I won this round")
        Return False
    End Function

    Sub PlayGame()
        While True
            Dim round As Boolean = PlayRound()

            ' Player won
            if(round = True)
                playerDeck.Add(computerDeck(0))
                playerDeck.Add(playerDeck(0))
                computerDeck.RemoveAt(0)
                playerDeck.RemoveAt(0)
                computer = False
            ' Compter won
            Else
                computerDeck.Add(playerDeck(0))
                computerDeck.Add(computerDeck(0))
                playerDeck.RemoveAt(0)
                computerDeck.RemoveAt(0)
                computer = True
            End If

            If(playerDeck.Count = 0)
                Console.WriteLine()
                Console.WriteLine("I win")
                Console.WriteLine()
                Exit While
            Else If(computerDeck.Count = 0)
                Console.WriteLine()
                Console.WriteLine("You win")
                Console.WriteLine()
                Exit While
            End If
        End While
    End Sub
End Module  
