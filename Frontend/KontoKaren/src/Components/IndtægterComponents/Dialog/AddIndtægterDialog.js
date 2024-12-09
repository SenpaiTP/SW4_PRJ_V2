import React, { useState } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, TextField } from '@mui/material';

export default function AddIncomeDialog({ open, handleClose, handleSave }) {
  const [name, setName] = useState('');
  const [price, setPrice] = useState('');
  const [date, setDate] = useState('');

  const handleSubmit = () => {
    if (name && price && date) {
      handleSave({ name, price, date });
      setName('');
      setPrice('');
      setDate('');
      handleClose(); 
    } else {
      alert('Alle felter skal udfyldes!');
    }
  };

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Tilføj ny Indtægt</DialogTitle>
      <DialogContent>
        <TextField
          label="Indtægtsnavn"
          variant="outlined"
          fullWidth
          value={name}
          onChange={(e) => setName(e.target.value)}
          sx={{ marginBottom: 2, mariginTop: 2 }}
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
