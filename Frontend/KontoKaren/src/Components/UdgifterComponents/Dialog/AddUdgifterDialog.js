import React, { useState } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, TextField } from '@mui/material';
import CategoryOption from '../Category/CategoryOption';

export default function AddExpenseDialog({ open, handleClose, handleSave, selectedCategory = '', setSelectedCategory }) {
  const [name, setName] = useState('');
  const [category, setCategory] = useState(selectedCategory); // Use the passed or default value
  const [price, setPrice] = useState('');
  const [date, setDate] = useState('');

  // Handle saving expense
  const handleSubmit = () => {
    if (name && category && price && date) {
      handleSave({ name, category, price, date });
      setName('');
      setCategory('');
      setPrice('');
      setDate('');
      handleClose();
    } else {
      alert('Alle felter skal udfyldes!');
    }
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Tilføj ny Udgift</DialogTitle>
      <DialogContent>
        <TextField
          label="Udgiftsnavn"
          variant="outlined"
          fullWidth
          value={name}
          onChange={(e) => setName(e.target.value)}
          sx={{ marginBottom: 2, mariginTop: 2 }}
        />

        {/* Pass setCategory and category to CategoryOption */}
        <CategoryOption
          onCategorySelect={(newCategory) => {
            setCategory(newCategory);  // Update category
          }}
          selectedCategory={category}  // Pass current category
          
        />

        <TextField
          label="Beløb (DKK)"
          variant="outlined"
          fullWidth
          value={price}
          onChange={(e) => setPrice(e.target.value)}
          sx={{ marginBottom: 2 }}
        />
        <TextField
          label="Dato"
          type="date"
          fullWidth
          value={date}
          onChange={(e) => setDate(e.target.value)}
          margin="normal"
          InputLabelProps={{
            shrink: true,
          }}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={handleSubmit} color="primary">
          Gem
        </Button>
        <Button onClick={handleClose} color="secondary">
          Annuller
        </Button>
      </DialogActions>
    </Dialog>
  );
}
