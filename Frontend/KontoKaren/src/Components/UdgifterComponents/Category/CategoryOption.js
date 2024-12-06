import React, { useState, useEffect } from 'react';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';
import { suggestCategory } from '../../../Services/CategoryService';
import useUdgifterHooks from '../../../Hooks/UseUdgifterHooks';
import { initialExpenseRows } from '../Table/UdgifterTableData';

const filter = createFilterOptions();

export default function CategorySet({ onCategorySelect, selectedCategory }) {
  const { rows } = useUdgifterHooks(initialExpenseRows); // Hent rows fra hooket
  const [categories, setCategories] = useState([]);
  const [categoryOptions, setCategoryOptions] = useState(loadCategoriesFromStorage()); // Initial categories
  const [category, setCategory] = useState(selectedCategory || ''); // Holder værdien fra kategori inputfeltet
  const [name, setName] = useState(''); // Holder værdien fra udgiftsnavn inputfeltet
  const [suggestedCategory, setSuggestedCategory] = useState(''); // Foreslået kategori

  // Load categories from localStorage or default categories
  function loadCategoriesFromStorage() {
    const savedCategories = localStorage.getItem('categories');
    return savedCategories ? JSON.parse(savedCategories) : [
      { categoryName: 'Tøj' },
      { categoryName: 'Mad' },
      { categoryName: 'Underholdning' },
    ];
  }

  useEffect(() => {
    // Persist categories in localStorage whenever categoryOptions changes
    localStorage.setItem('categories', JSON.stringify(categoryOptions));
  }, [categoryOptions]);

  const handleSuggestCategories = (description) => {
    if (!description || description.trim() === '') {
      console.error('Ingen tekst angivet til kategoriforslag.');
      return;
    }

    suggestCategory(description.trim())
      .then((category) => {
        if (category) {
          console.log(`Foreslået kategori: ${category}`);

          // Tjek, om kategorien allerede eksisterer
          const exists = categoryOptions.some(
            (cat) => cat.categoryName.toLowerCase() === category.toLowerCase()
          );
          if (!exists) {
            setCategoryOptions((prevOptions) => [
              ...prevOptions,
              { categoryName: category },
            ]);
          }
        }
      })
      .catch((error) => console.error('Fejl ved foreslåelse af kategori:', error));
  };

  // Når inputfeltet for udgiftsnavn ændres
  const handleUdgiftsnavnChange = (e) => {
    const newName = e.target.value;
    setName(newName); // Opdaterer 'name' state med inputværdi
  };

  return (
    <>
      {/* Udgiftsnavn TextField */}
      <TextField
        label="Udgiftsnavn"
        variant="outlined"
        fullWidth
        value={name} // Bruger 'name' som værdien af inputfeltet
        onChange={handleUdgiftsnavnChange} // Opdaterer 'name' state uden at foreslå kategorier
        sx={{ marginBottom: 2, marginTop: 2 }}
      />

      <Autocomplete
        value={category}
        onChange={(event, newValue) => {
          setCategory(newValue?.categoryName || ''); // Opdaterer kategori
          if (onCategorySelect && newValue?.categoryName) {
            onCategorySelect(newValue.categoryName); // Opdaterer den valgte kategori i forældrekomponenten
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
        options={[...categoryOptions, ...(suggestedCategory ? [{ categoryName: suggestedCategory }] : [])]} // Add suggested category if available
        getOptionLabel={(option) => {
          if (typeof option === 'string') {
            return option;
          }
          if (option.inputValue) {
            return option.inputValue;
          }
          return option.categoryName;
        }}
        selectOnFocus
        clearOnBlur
        handleHomeEndKeys
        renderOption={(props, option) => <li {...props}>{option.categoryName}</li>}
        freeSolo
        renderInput={(params) => (
          <TextField
            {...params}
            label="Kategori"
            fullWidth
          />
        )}
      />

      {/* Button to suggest categories */}
      <Button
        variant="contained"
        onClick={() => handleSuggestCategories(name)} // Forslår kategorier baseret på 'name'
        disabled={!name || name.trim() === ''} // Disable knappen, hvis name er tomt
        sx={{ marginTop: 2 }}
      >
        Foreslå Kategorier
      </Button>

      {/* Dialog for adding a new category */}
      <Dialog open={false} onClose={() => {}}>
        <form onSubmit={() => {}}>
          <DialogTitle>Tilføj ny kategori</DialogTitle>
          <DialogContent>
            <TextField
              autoFocus
              margin="dense"
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              label="Kategori"
              type="text"
              variant="standard"
            />
          </DialogContent>
          <DialogActions>
            <Button type="submit">Tilføj</Button>
            <Button onClick={() => {}}>Annuller</Button>
          </DialogActions>
        </form>
      </Dialog>
    </>
  );
}
