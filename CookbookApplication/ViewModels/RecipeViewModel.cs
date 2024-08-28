using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using CookbookApplication.Services;
using CookbookApplication.Models;
using CookbookApplication.Views;
using static CookbookApplication.Services.DefaultDialogService;
using NPOI.POIFS.Properties;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using System.Windows.Input;

namespace CookbookApplication.ViewModels
{
    internal class RecipeViewModel : INotifyPropertyChanged
    {
        private Recipe selectedRecipe;
        private string? searchText;

        public ObservableCollection<Recipe> Recipes { get; set; }
        public ObservableCollection<Recipe> FilteredRecipes { get; set; }

        private readonly IDialogService dialogService;
        private readonly IFileService pdfFileService;
        private readonly IFileService docxFileService;
        private readonly IFileService jsonFileService;

        public Recipe SelectedRecipe
        {
            get => selectedRecipe;
            set
            {
                selectedRecipe = value;
                OnPropertyChanged();
            }
        }

        public string? SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                if (SearchText == string.Empty)
                {
                    SearchParameters(new object());
                }
            }
        }

        public List<string> RecipeTypeNames { get; set; } =
        [
            "Main-dish",
            "Side-dish",
            "Dessert",
            "Breakfast",
            "Lunch",
            "Dinner",
            "Salad",
            "Soup",
            "Stew",
            "Snack",
            "Baked-goods",
            "Others"
        ];

        public List<string> RecipeCuisineNames { get; set; } =
        [
            "American",
            "British",
            "Chinese",
            "French",
            "Japanese",
            "Khmer",
            "Mexican",
            "Spanish",
            "Thai",
            "Vietnamese",
            "Others"
        ];

        public RecipeViewModel(IDialogService dialogService, IFileService docxFileService, IFileService pdfFileService, IFileService jsonFileService)
        {
            this.dialogService = dialogService;
            this.pdfFileService = pdfFileService;
            this.docxFileService = docxFileService;
            this.jsonFileService = jsonFileService;

            Recipes =
            [
                new Recipe
                {
                    Name = "Fried_Rice",
                    Type = "Breakfast",
                    Cuisine = "Khmer",
                    Ingredients = [new Ingredient { Name = "rice", Quantity = "100g"},
                                   new Ingredient { Name = "egg" , Quantity = "1 whole"},
                                   new Ingredient { Name = "beef" , Quantity = "3g"},],
                    Instructions = [new Instruction { Name = "crack egg"},
                                    new Instruction { Name = "stir"}],
                    ImagePath = "..\\Resources\\Chinese-Sausage-Egg-Fried-Rice4.jpg" },
                new Recipe
                {
                    Name = "Curry Chicken",
                    Type = "Soup",
                    Cuisine = "Cambodia",
                    Ingredients = [new Ingredient { Name = "garlic", Quantity = "100g"},
                                   new Ingredient { Name = "lemongrass" , Quantity = "1"},
                                   new Ingredient { Name = "galangal" , Quantity = "1"},
                                   new Ingredient { Name = "palm sugar" , Quantity = "1"},
                                   new Ingredient { Name = "egg" , Quantity = "1 whole" }],
                    Instructions = [new Instruction { Name = "Adjust the oven rack n to the top position and preheat the broiler to high." },
                                    new Instruction { Name = "Cover a large baking sheet with parchment paper or foil. Slice the chicken breasts crosswise and cut into bite-sized pieces (¾-inch or so)."}],
                    ImagePath = "..\\Resources\\our-local-s-signature.jpg" },
                new Recipe
                {
                    Name = "Soup Beef",
                    Type = "dinner",
                    Cuisine = "Khmer",
                    Ingredients = [new Ingredient { Name = "tablespoons", Quantity = "1"},
                                   new Ingredient { Name = "cups of water" , Quantity = "6"},
                                   new Ingredient { Name = "yellow onion" , Quantity = "1"},
                                   new Ingredient { Name = "large white potato" , Quantity = "6"},
                                   new Ingredient { Name = "Beef chuck or round beef" , Quantity = "450g"},
                                   new Ingredient { Name = "egg" , Quantity = "1 whole"}],
                    Instructions = [new Instruction { Name = "Heat up a large soup pot.When it hot add oil, garlic, onion, beef and soy sauce, stirs well."},
                                    new Instruction { Name = "Add water and cook till beef tender."},
                                    new Instruction { Name = "Seasoning with fish sauce, sugar and black pepper." },
                                    new Instruction { Name = "Add potato and cook till potato tender."}],
                    ImagePath = "..\\Resources\\Beef-shank-and-green-papaya-soup.jpg" },
                new Recipe
                {
                    Name = "Fried Noodle",
                    Type = "snack",
                    Cuisine = "Khmer",
                    Ingredients = [new Ingredient { Name = "Noodle", Quantity = "100g"},
                                   new Ingredient { Name = "egg" , Quantity = "1 whole"},
                                   new Ingredient { Name = "Beef" , Quantity = "3g"}],
                    Instructions = [new Instruction { Name = "Once the noodles are drained, I like to season them with dark soy sauce. This step is optional. I like to use about 1 tablespoon to taste.\r\n"},
                                    new Instruction { Name = "I like to prepare my pin noodles by flash cooking them in boiling hot water for 1-2 minutes before straining them and setting aside. This helps loosen the noodles before cooking."}],
                    ImagePath = "..\\Resources\\cambodian-fried-noodles-with-chicken-S076A1.jpg" },
                new Recipe
                {
                    Name = "spaghetti",
                    Type = "snack",
                    Cuisine = "Italy",
                    Ingredients = [new Ingredient { Name = "small onion", Quantity = "200g"},
                                   new Ingredient { Name = "egg" , Quantity = "1 whole"},
                                   new Ingredient { Name = "bell pepper" , Quantity = "1"},
                                   new Ingredient { Name = "tablespoons butter" , Quantity = "1"},
                                   new Ingredient { Name = "tablespoons salt" , Quantity = "1"},
                                   new Ingredient { Name = "tablespoons garlic" , Quantity = "1"}],
                    Instructions = [new Instruction { Name = "On medium heat melt the butter and sautee the onion and bell peppers."},
                                    new Instruction { Name = "Add the hamburger meat and cook until meat is well done."},
                                    new Instruction { Name = "Add the tomato sauce, salt, pepper and garlic powder."},
                                    new Instruction { Name = "Salt, pepper and garlic powder can be adjusted to your own tastes."}],
                    ImagePath = "..\\Resources\\Easyspaghettiwithtomatosauce_11715_DDMFS_1x2_2425-c67720e4ea884f22a852f0bb84a87a80.jpg" },
                new Recipe
                {
                    Name = "Tong Yum",
                    Type = "Breakfast",
                    Cuisine = "Thai",
                    Ingredients = [new Ingredient { Name = "roma tomato", Quantity = "1"},
                                   new Ingredient { Name = "white onion" , Quantity = "1 whole"},
                                   new Ingredient { Name = "oyster mushrooms" , Quantity = "120g"},],
                    Instructions = [new Instruction { Name = "peel the prawn.Place headss and shell in pot"},
                                    new Instruction { Name = "Use a meat mallet or similar to bash the garlic, chilli and lemongrass so they burst open to release flavour. Add into pot."},
                                    new Instruction { Name = "Add galangal, stock and water. Bring to simmer on high heat, cover, then reduce to medium and simmer for 10 minutes." }],
                    ImagePath = "..\\Resources\\Tom-Yum-Soup.jpg" },
                new Recipe
                {
                    Name = "Salad",
                    Type = "Breakfast",
                    Cuisine = "French",
                    Ingredients = [new Ingredient { Name = "cherry tomato", Quantity = "1"},
                                   new Ingredient { Name = "Mix vegetable" , Quantity = "1 whole"},
                                   new Ingredient { Name = "black pepper" , Quantity = "120g"},],
                    Instructions = [new Instruction { Name = "Make the dressing: Zest the lemon into a small bowl, then halve the naked lemon and squeeze the juice into the bowl."},
                                    new Instruction { Name = "Put the romaine into a large bowl, then top with tomatoes, bacon, cucumber, blue cheese and scallions."}],
                    ImagePath = "..\\Resources\\merlin_209652387_1b5eee4c-9da5-443c-90e0-db20ee51a246-superJumbo.jpg" },
            ];
            FilteredRecipes = new(Recipes);

            AddRecipeCommand = new(AddRecipe);
            RemoveRecipeCommand = new(RemoveRecipe, CanRemoveRecipe);
            EditRecipeCommand = new(EditRecipe, CanEditRecipe);
            ExportRecipeCommand = new(ExportRecipe, CanExportRecipe);

            AddIngredientCommand = new(AddIngredient);
            AddInstructionCommand = new(AddInstruction);
            RemoveIngredientCommand = new(RemoveIngredient, CanRemoveIngredient);
            RemoveInstructionCommand = new(RemoveInstruction, CanRemoveInstruction);
            EditImageCommand = new(EditImage);

            SearchParameterCommand = new(SearchParameters);

            SaveDocDocxFileCommand = new(SaveDocDocxFile);
            SavePdfFileCommand = new(SavePdfFile);
            SaveJsonFileCommand = new(SaveJsonFile);

            OpenJsonFileCommand = new(OpenJsonFile);
        }

        public RelayCommand AddRecipeCommand { get; }
        public RelayCommand RemoveRecipeCommand { get; }
        public RelayCommand EditRecipeCommand { get; }
        public RelayCommand ExportRecipeCommand { get; }

        public RelayCommand AddIngredientCommand { get; }
        public RelayCommand RemoveIngredientCommand { get; }
        public RelayCommand AddInstructionCommand { get; }
        public RelayCommand RemoveInstructionCommand { get; }
        public RelayCommand EditImageCommand { get; }

        public RelayCommand SearchParameterCommand { get; }

        public RelayCommand SaveDocDocxFileCommand { get; }
        public RelayCommand SavePdfFileCommand { get; }
        public RelayCommand SaveJsonFileCommand { get; }

        public RelayCommand OpenJsonFileCommand { get; }

        private void AddRecipe(object parameter)
        {
            Recipes.Add(new Recipe
            {
                Name = "New Name",
                Type = "New Type",
                Cuisine = "New Cuisine",
                ImagePath = "..\\Resources\\image.png",
                Ingredients = [new Ingredient { Name = "New Ingredient", Quantity = "1" }],
                Instructions = [new Instruction { Name = "New Instruction" }]
            });
            SearchParameters(new object());
        }

        private bool CanRemoveRecipe(object parameter)
        {
            return SelectedRecipe != null;
        }

        private void RemoveRecipe(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Delete the recipe?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result == MessageBoxResult.Yes)
            {
                Recipes.Remove(SelectedRecipe);
                SearchParameters(new object());
            }
        }


        private void EditRecipe(object parameter)
        {
            RecipeEditView recipeEditView = new()
            {
                DataContext = this
            };
            recipeEditView.ShowDialog();
        }

        private bool CanEditRecipe(object parameter)
        {
            return SelectedRecipe != null;
        }

        private void ExportRecipe(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(DefaultDialogService.FileType.All))
                {
                    List<Recipe> list = [SelectedRecipe];

                    string filePath = dialogService.FilePath;
                    string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                    if (fileExtension == ".doc" || fileExtension == ".docx")
                    {
                        docxFileService.Save(filePath, list);
                    }
                    else if (fileExtension == ".pdf")
                    {
                        pdfFileService.Save(filePath, list);
                    }
                    else
                    {
                        dialogService.ShowMessage("Unsupported file type.");
                        return;
                    }

                    dialogService.ShowMessage("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        private bool CanExportRecipe(object parameter)
        {
            return SelectedRecipe != null;
        }

        private void AddIngredient(object parameter)
        {
            SelectedRecipe?.Ingredients?.Add(new Ingredient { Name = "New Ingredient", Quantity = "1" });
        }

        private void RemoveIngredient(object parameter)
        {
            if (parameter is Ingredient ingredient && SelectedRecipe != null)
            {
                SelectedRecipe.Ingredients?.Remove(ingredient);
            }
        }

        private bool CanRemoveIngredient(object parameter)
        {
            return parameter is Ingredient ingredient && SelectedRecipe?.Ingredients?.Contains(ingredient) == true;
        }

        private void AddInstruction(object parameter)
        {
            SelectedRecipe?.Instructions?.Add(new Instruction { Name = "New Instruction" });
        }

        private void RemoveInstruction(object parameter)
        {
            if (parameter is Instruction instruction && SelectedRecipe != null)
            {
                SelectedRecipe?.Instructions?.Remove(instruction);
            }
        }

        private bool CanRemoveInstruction(object parameter)
        {
            return parameter is Instruction instruction && SelectedRecipe?.Instructions?.Contains(instruction) == true;
        }

        private void EditImage(object parameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                SelectedRecipe.ImagePath = openFileDialog.FileName;
                OnPropertyChanged(nameof(SelectedRecipe));
            }
        }

        private void SearchParameters(object parameter)
        {
            List<string>? searchParameters = SearchText?
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(term => term.ToLower())
                .ToList();

            if (searchParameters == null || searchParameters.Count == 0)
            {
                FilteredRecipes.Clear();
                foreach (Recipe recipe in Recipes)
                {
                    FilteredRecipes.Add(recipe);
                }
                return;
            }

            FilteredRecipes.Clear();

            foreach (Recipe recipe in Recipes)
            {
                if (searchParameters.Any(searchString =>
                    recipe.Name?.ToLower().Contains(searchString) == true ||
                    recipe.Type?.ToLower().Contains(searchString) == true ||
                    recipe.Cuisine?.ToLower().Contains(searchString) == true))
                {
                    if (!FilteredRecipes.Contains(recipe))
                    {
                        FilteredRecipes.Add(recipe);
                    }
                }
            }
        }

        private void SaveDocDocxFile(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(FileType.Word))
                {
                    docxFileService.Save(dialogService.FilePath,
                        Recipes.Select(recipe => new Recipe
                        { Name = recipe?.Name, Type = recipe?.Type, Cuisine = recipe?.Cuisine, ImagePath = recipe?.ImagePath,
                            Ingredients = recipe?.Ingredients, Instructions = recipe?.Instructions }).ToList());
                    dialogService.ShowMessage("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        private void SavePdfFile(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(FileType.Pdf))
                {
                    pdfFileService.Save(dialogService.FilePath,
                        Recipes.Select(recipe => new Recipe
                        { Name = recipe?.Name, Type = recipe?.Type, Cuisine = recipe?.Cuisine, ImagePath = recipe?.ImagePath,
                            Ingredients = recipe?.Ingredients, Instructions = recipe?.Instructions }).ToList());
                    dialogService.ShowMessage("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }

        private void OpenJsonFile(object parameter)
        {
            if (dialogService.OpenFileDialog())
            {
                List<Recipe> recipeList = jsonFileService.Open(dialogService.FilePath);
                Recipes = new(recipeList);
                FilteredRecipes.Clear();
                foreach (Recipe recipe in Recipes)
                {
                    FilteredRecipes.Add(recipe);
                }
                return;
            }
        }

        private void SaveJsonFile(object parameter)
        {
            try
            {
                if (dialogService.SaveFileDialog(FileType.Json))
                {
                    jsonFileService.Save(dialogService.FilePath,
                        Recipes.Select(recipe => new Recipe
                        {
                            Name = recipe?.Name,
                            Type = recipe?.Type,
                            Cuisine = recipe?.Cuisine,
                            ImagePath = recipe?.ImagePath,
                            Ingredients = recipe?.Ingredients,
                            Instructions = recipe?.Instructions
                        }).ToList());
                    dialogService.ShowMessage("File saved successfully.");
                }
            }
            catch (Exception ex)
            {
                dialogService.ShowMessage(ex.Message);
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
