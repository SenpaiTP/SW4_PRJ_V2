import React, { useState, useEffect } from 'react';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';

const filter = createFilterOptions();

export default function CategoryOption({ onCategorySelect }) {
  // Load categories from localStorage on component mount
  const loadCategoriesFromStorage = () => {
    const savedCategories = localStorage.getItem('categories');
    return savedCategories ? JSON.parse(savedCategories) : [
      { categoryName: 'Tøj' },
      { categoryName: 'Mad' },
      { categoryName: 'Underholdning' },
    ];
  };

  const [categoryOptions, setCategoryOptions] = useState(loadCategoriesFromStorage()); // Initial categories from localStorage or default
  const [value, setValue] = useState(null);
  const [open, toggleOpen] = useState(false);
  const [dialogValue, setDialogValue] = useState({ categoryName: '' });

  useEffect(() => {
    // Persist categories in localStorage whenever categoryOptions changes
    localStorage.setItem('categories', JSON.stringify(categoryOptions));
  }, [categoryOptions]);

  const handleClose = () => {
    setDialogValue({ categoryName: '' });
    toggleOpen(false);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    if (dialogValue.categoryName.trim()) {
      const newCategory = { categoryName: dialogValue.categoryName };
      setCategoryOptions((prevOptions) => [...prevOptions, newCategory]);

      if (onCategorySelect) {
        onCategorySelect(newCategory.categoryName);
      }

      handleClose(); // Close the dialog after adding
    }
  };

  return (
    <>
      <Autocomplete
        value={value}
        onChange={(event, newValue) => {
          if (typeof newValue === 'string') {
            setTimeout(() => {
              toggleOpen(true);
              setDialogValue({ categoryName: newValue });
            });
          } else if (newValue && newValue.inputValue) {
            toggleOpen(true);
            setDialogValue({ categoryName: newValue.inputValue });
          } else {
            setValue(newValue);
            if (onCategorySelect && newValue?.categoryName) {
              onCategorySelect(newValue.categoryName);
            }
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
        options={categoryOptions}
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
        renderInput={(params) => <TextField {...params} label="Kategori" />}
        label="Udgiftsnavn"
        variant="outlined"
        fullWidth
        sx={{ marginBottom: 2, mariginTop: 2 }}
      />

      {/* Dialog for adding a new category */}
      <Dialog open={open} onClose={handleClose}>
        <form onSubmit={handleSubmit}>
          <DialogTitle>Tilføj ny kategori</DialogTitle>
          <DialogContent>
            <TextField
              autoFocus
              margin="dense"
              id="name"
              value={dialogValue.categoryName}
              onChange={(event) =>
                setDialogValue({
                  ...dialogValue,
                  categoryName: event.target.value,
                })
              }
              label="Kategori"
              type="text"
              variant="standard"
            />
          </DialogContent>
          <DialogActions>
            <Button type="submit">Tilføj</Button>
            <Button onClick={handleClose}>Annuller</Button>
          </DialogActions>
        </form>
      </Dialog>
    </>
  );
}
