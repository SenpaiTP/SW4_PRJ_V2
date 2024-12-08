import React, { useState } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, TextField } from '@mui/material';
import CategoryOption from '../Category/CategoryOption';

export default function AddExpenseDialog({ open, handleClose, handleSave, selectedCategory = '', setSelectedCategory }) {
  const [name, setName] = useState('');
  const [category, setCategory] = useState(selectedCategory); // Use the passed or default value
  const [price, setPrice] = useState('');
  const [date, setDate] = useState('');
  const [errors, setErrors] = useState({ name: '', category: '', price: '', date: '' });

  const handleSubmit = () => {
    let valid = true;
    let newErrors = { name: '', category: '', price: '', date: '' };

    if (!name || typeof name !== 'string') {
      newErrors.name = 'Udgiftsnavn er påkrævet og skal være en tekst';
      valid = false;
    }

    if (!category) {
      newErrors.category = 'Kategori er påkrævet';
      valid = false;
    }

    if (!price || isNaN(price)) {
      newErrors.price = 'Beløb skal være et tal';
      valid = false;
    }

    if (!date) {
      newErrors.date = 'Dato er påkrævet';
      valid = false;
    }

    setErrors(newErrors);

    if (valid) {
      handleSave({ name, category, price: parseFloat(price), date });
      setName('');
      setCategory('');
      setPrice('');
      setDate('');
      handleClose();
    } else {
      console.error('Manglende felter:', {
        nameMissing: !name,
        categoryMissing: !category,
        priceMissing: !price,
        dateMissing: !date,
      });
      alert('Alle felter skal udfyldes!');
    }
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Tilføj ny Udgift</DialogTitle>
      <DialogContent>
        <CategoryOption
          onCategorySelect={(newCategory) => {
            console.log('Kategori valgt:', newCategory); // Debugging
            setCategory(newCategory);  // Update category
          }}
          onNameChange={(newName) => {
            console.log('Navn ændret:', newName); // Debugging
            setName(newName);  // Update name
          }}
          selectedCategory={category}  // Pass current category
        />
        <TextField
          label="Beløb (DKK)"
          variant="outlined"
          fullWidth
          value={price}
          onChange={(e) => {
            console.log('Beløb ændret: ', e.target.value);
            setPrice(e.target.value);
          }}
          sx={{ marginBottom: 2 }}
          error={!!errors.price}
          helperText={errors.price}
        />
        <TextField
          label="Dato"
          type="date"
          fullWidth
          value={date}
          onChange={(e) => {
            console.log('Dato ændret: ', e.target.value);
            setDate(e.target.value);
          }}
          margin="normal"
          InputLabelProps={{
            shrink: true,
          }}
          error={!!errors.date}
          helperText={errors.date}
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