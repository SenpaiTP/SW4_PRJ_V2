import React, { useState } from 'react';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';

const filter = createFilterOptions();

export default function CategoryOption({ onCategorySelect }) {
  const [categoryOptions, setCategoryOptions] = useState([
    { categoryName: 'Tøj' },
    { categoryName: 'Mad' },
    { categoryName: 'Underholdning' },
  ]); // Initial categories in state

  const [value, setValue] = useState(null); // Track the selected value
  const [open, toggleOpen] = useState(false); // Manage the dialog open state
  const [dialogValue, setDialogValue] = useState({
    categoryName: '',
  });

  const handleClose = () => {
    setDialogValue({ categoryName: '' });
    toggleOpen(false);
  };

  // Handle adding a new category
  const handleSubmit = (event) => {
    event.preventDefault();
    if (dialogValue.categoryName.trim()) {
      const newCategory = { categoryName: dialogValue.categoryName };
      // Add the new category to the options list
      setCategoryOptions((prevOptions) => [...prevOptions, newCategory]);

      // Pass the new category to the parent
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
              onCategorySelect(newValue.categoryName); // Select a category
            }
          }
        }}
        filterOptions={(options, params) => {
          const filtered = filter(options, params);

          if (params.inputValue !== '') {
            filtered.push({
              inputValue: params.inputValue,
              categoryName: `Tilføj "${params.inputValue}"`, // Suggest adding new category
            });
          }

          return filtered;
        }}
        options={categoryOptions} // Dynamically update the list of categories
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
