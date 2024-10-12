# Automaton

## Description

Le projet **Automaton** vise à développer un automate qui reconnaît si un mot appartient à un langage donné. Il inclut également la construction d'un automate qui reconnaît l'intersection de deux langages à partir de deux automates distincts.

## Fonctionnalités

- [x] **Affichage des états et des transitions** : Présente les états et les transitions de l'automate.
- [x] **Reconnaissance de langage** : Vérifie si un mot appartient à un langage défini par un automate.
- [x] **Produit d'automate** : Génère un automate qui reconnaît l'intersection de deux langages provenant de deux automates distincts.

### Futures fonctionnalités

- [ ] **Union de langages** : Implémenter la création d'un automate qui reconnaît l'union de deux langages.
- [ ] **Compléter un automate** : Ajouter un état puits si nécessaire pour compléter l'automate.
- [ ] **Complémentaire d'un langage** : À partir d'un automate déterministe, retourner un automate reconnaissant le complémentaire du langage.
- [ ] **Conversion non déterministe à déterministe** : Prendre un automate non déterministe et retourner un automate déterministe reconnaissant le même langage.
- [ ] **Vérification de la vacuité** : Déterminer si le langage reconnu par un automate est vide.
- [ ] **Vérification de l'infinité** : Déterminer si le langage reconnu par un automate est infini.


## Installation

1. Clonez le repository :
   ```bash
   git clone https://github.com/votre-utilisateur/Automaton.git
   cd Automaton
   ```

2. Ouvrez le projet dans votre environnement de développement intégré (IDE) préféré.

3. Compilez et exécutez le projet.

## Utilisation

### 1. Créer un automate

Pour créer un automate, définissez ses états et ses transitions. Par exemple :

```csharp
State[] states =
{
    new State(1, true),  // État d'entrée
    new State(2),
    new State(3, false, true),  // État de sortie
};

Transition[] transitions =
{
    new Transition(states[0], states[1], 'a'),
    new Transition(states[1], states[2], 'b'),
};

Automate automate = new Automate(states, transitions);
```

### 2. Vérifier un mot

Utilisez la méthode `Is_Recognized` pour vérifier si un mot appartient à l'automate :

```csharp
var word = "ab";
bool recognized = automate.Is_Recognized(word);
Console.WriteLine(recognized ? $"{word} appartient au langage" : $"{word} n'appartient pas au langage");
```

### 3. Intersection de deux automates

Pour créer un automate qui reconnaît l'intersection de deux langages, utilisez la méthode `Intersection` :

```csharp
Automate intersectedAutomate = Automate.Intersection(automateA, automateB);
```

## Contribution

Les contributions sont les bienvenues ! Veuillez soumettre une demande de tirage (pull request) ou ouvrir une issue pour discuter des améliorations.

## License

Ce projet est sous licence MIT. Consultez le fichier LICENSE pour plus de détails.

## Acknowledgments

- [Automates et Langages](https://fr.wikipedia.org/wiki/Automate_fini) - Source d'inspiration pour la théorie des automates.
- Merci à tous les contributeurs qui ont aidé à faire avancer ce projet !

### Personnalisation

N'hésitez pas à personnaliser davantage le README en fonction de votre projet, en ajoutant des informations spécifiques, des exemples supplémentaires, ou des instructions particulières si nécessaire. Si vous avez besoin d'autres ajustements ou sections, faites-le moi savoir !
