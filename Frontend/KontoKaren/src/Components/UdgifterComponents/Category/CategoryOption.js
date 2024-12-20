import React, { useState, useEffect } from 'react';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';
import IconButton from '@mui/material/IconButton';
import DeleteIcon from '@mui/icons-material/Delete';
import { suggestCategory } from '../../../Services/CategoryService';

const filter = createFilterOptions();

export default function CategorySet({ onCategorySelect, selectedCategory, onNameChange, initialName }) {
  const [categoryOptions, setCategoryOptions] = useState(loadCategoriesFromStorage()); 
  const [category, setCategory] = useState(selectedCategory || '');
  const [name, setName] = useState(initialName || '');
  const [suggestedCategory, setSuggestedCategory] = useState(''); // foreslået kategori
  const [suggestedCategoryText, setSuggestedCategoryText] = useState(''); // vis tekst for foreslået kategori

  // load categories fra localStorage eller default categories
  function loadCategoriesFromStorage() {
    const savedCategories = localStorage.getItem('categories');
    return savedCategories
      ? JSON.parse(savedCategories)
      : [
          { categoryName: 'Tøj' },
          { categoryName: 'Mad' },
          { categoryName: 'Underholdning' },
        ];
  }

  // fjern dubletter og gem kategorier
  useEffect(() => {
    const uniqueCategories = categoryOptions.filter(
      (value, index, self) =>
        index ===
        self.findIndex(
          (t) =>
            t.categoryName.toLowerCase() === value.categoryName.toLowerCase()
        )
    );

    // gem unikke kategorier i localStorage
    localStorage.setItem('categories', JSON.stringify(uniqueCategories));
  }, [categoryOptions]);

  // opdater kategori baseret på forældreprops
  useEffect(() => {
    console.log('Opdaterer kategori til:', selectedCategory);
    setCategory(selectedCategory || '');
  }, [selectedCategory]);

  // foreslå kategorier baseret på beskrivelse
  const handleSuggestCategories = (description) => {
    if (!description || description.trim() === '') {
      console.error('Ingen tekst angivet til kategoriforslag.');
      return;
    }

    console.log('Forslår kategori for:', description.trim());
    suggestCategory(description.trim())
      .then((category) => {
        console.log('Kategori foreslået:', category);
        if (category) {
          setSuggestedCategoryText(category);
          setSuggestedCategory(category);

          const exists = categoryOptions.some(
            (cat) => cat.categoryName.toLowerCase() === category.toLowerCase()
          );

          if (!exists) {
            setCategoryOptions((prevOptions) => [
              ...prevOptions,
              { categoryName: category },
            ]);
            console.log('Kategori tilføjet til options:', category);
          }
        }
      })
      .catch((error) =>
        console.error('Fejl ved foreslåelse af kategori:', error)
      );
  };

  // når inputfeltet for udgiftsnavn ændres
  const handleUdgiftsnavnChange = (e) => {
    let newName = e.target.value;
    console.log('Udgiftsnavn ændret til:', newName);
  
    const formattedName = newName
      .split(' ') 
      .map(word => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase()) // gør første bogstav stort
      .join(' '); 
  
    setName(formattedName); 
  
    if (onNameChange) {
      onNameChange(formattedName); 
    }
  };
  

  // når knappen til at tilføje foreslået kategori trykkes
  const handleAddSuggestedCategory = () => {
    if (suggestedCategory) {
      setCategory(suggestedCategory); // tilføj den foreslåede kategori til kategori-inputfeltet
      setSuggestedCategoryText(''); // fjern den foreslåede tekst
      if (onCategorySelect) {
        onCategorySelect(suggestedCategory);
      }
    }
  };

  // fandle delete category
  const handleDeleteCategory = (categoryToDelete) => {
    const updatedCategories = categoryOptions.filter(
      (category) => category.categoryName !== categoryToDelete
    );
    setCategoryOptions(updatedCategories);
    localStorage.setItem('categories', JSON.stringify(updatedCategories));
    console.log('Kategori slettet:', categoryToDelete);
  };

  return (
    <>
      {/* Udgiftsnavn TextField */}
      <TextField
        label="Udgiftsnavn"
        variant="outlined"
        fullWidth
        value={name}
        onChange={handleUdgiftsnavnChange}
        sx={{ marginBottom: 2, marginTop: 2 }}
      />

      {/* Autocomplete til kategorier */}
      <Autocomplete
        value={category}
        onChange={(event, newValue) => {
          setCategory(newValue?.categoryName || '');
          console.log('Kategori ændret til:', newValue?.categoryName);
          if (onCategorySelect && newValue?.categoryName) {
            onCategorySelect(newValue.categoryName);
          }
        }}
        filterOptions={(options, params) => {
          const filtered = filter(options, params);
          if (params.inputValue !== '') {
            filtered.push({
              inputValue: params.inputValue,
              categoryName: `Tilføj "${params.inputValue}"`,
            });
          }
          return filtered;
        }}
        options={[
          ...categoryOptions,
          ...(suggestedCategory ? [{ categoryName: suggestedCategory }] : []),
        ]}
        getOptionLabel={(option) =>
          typeof option === 'string'
            ? option
            : option.inputValue || option.categoryName
        }
        selectOnFocus
        clearOnBlur
        handleHomeEndKeys
        renderOption={(props, option) => (
          <li {...props} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            {option.categoryName}
            <IconButton
              edge="end"
              aria-label="delete"
              onClick={() => handleDeleteCategory(option.categoryName)}
            >
              <DeleteIcon />
            </IconButton>
          </li>
        )}
        freeSolo
        renderInput={(params) => (
          <TextField {...params} label="Kategori" fullWidth />
        )}
      />

      {/* Button til at foreslå categories */}
      <Button
        variant="contained"
        onClick={() => handleSuggestCategories(name)}
        disabled={!name || name.trim() === ''}
        sx={{ marginTop: 2 }}
      >
        Foreslå Kategorier
      </Button>

      {/* Display the suggested category with an 'Add' button */}
      {suggestedCategory && (
        <div style={{ marginTop: 16, display: 'flex', alignItems: 'center' }}>
          <TextField
            label="Foreslået Kategori"
            variant="outlined"
            fullWidth
            value={suggestedCategoryText}
            InputProps={{
              readOnly: true,
            }}
            sx={{ marginRight: 2 }}
          />
          <Button
            variant="contained"
            onClick={handleAddSuggestedCategory}
            disabled={!suggestedCategory}
          >
            Tilføj
          </Button>
        </div>
      )}
    </>
  );
}