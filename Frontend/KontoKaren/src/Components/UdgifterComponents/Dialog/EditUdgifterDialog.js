import React, { useState, useEffect } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, TextField } from '@mui/material';
import CategorySet from '../Category/CategoryOption';

export default function EditExpenseDialog({ open, handleClose, handleSave, expense }) {
  const [name, setName] = useState('');
  const [category, setCategory] = useState('');
  const [price, setPrice] = useState('');
  const [date, setDate] = useState('');
  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (open && expense) {
      console.log('Opdaterer udgift:', expense);
      setName(expense.name);
      setCategory(expense.category);
      setPrice(expense.price);
      setDate(expense.date);
    }
  }, [expense, open]);

  const handleSubmit = () => {
    const newErrors = {};
    if (!name) newErrors.name = 'Navn er påkrævet';
    if (!category) newErrors.category = 'Kategori er påkrævet';
    if (!price || isNaN(price) || parseFloat(price) <= 0) newErrors.price = 'Beløb skal være et tal';
    if (!date) newErrors.date = 'Dato er påkrævet';
  
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      alert('Alle felter skal udfyldes korrekt!');
      return;
    }
  
    console.log('Submit Data:', { id: expense.id, name, category, price, date });
    if (expense.id) {
      handleSave({ id: expense.id, name, category, price: parseFloat(price), date });
      handleClose();
    }
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Rediger Udgift</DialogTitle>
      <DialogContent>
        {/* Use CategorySet for editing category and Udgiftsnavn */}
        <CategorySet
          onCategorySelect={(newCategory) => {
            setCategory(newCategory);
          }}
          onNameChange={(newName) => {
            setName(newName);
          }}
          selectedCategory={category}
          initialName={name}
        />

        <TextField
          label="Beløb (DKK)"
          variant="outlined"
          fullWidth
          value={price}
          onChange={(e) => setPrice(e.target.value)}
          sx={{ marginBottom: 2 }}
          error={!!errors.price}
          helperText={errors.price}
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