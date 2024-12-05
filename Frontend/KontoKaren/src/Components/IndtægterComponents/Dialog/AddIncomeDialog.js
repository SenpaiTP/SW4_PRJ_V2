import React, { useState } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, TextField } from '@mui/material';
import fetchIncomeById from './FetchById';

export default function AddIncomeDialog({ open, handleClose, handleSave }) {
  const [name, setName] = useState('');
  const [price, setPrice] = useState('');
  const [date, setDate] = useState('');
  const [kategoriNavn, setKategoriNavn] = useState('');
  const [kategoriId, setKategoriId] = useState('');

  const handleSubmit = async () => {
    if (name && price && date) {
      const income = {
        Tekst: name,
        Indtægt: price,
        Dato: date,
        KategoriNavn: kategoriNavn,
        KategoriId: kategoriId,
      };
      const PieIncome ={
        name, price, date
      }

      handleIndtægt(income);
      handleSave(PieIncome);
      setName('');
      setPrice('');
      setDate('');
      setKategoriNavn('');
      setKategoriId('');
      handleClose(); 
    } else {
      alert('Alle felter skal udfyldes!');
    }
  };
  const token = localStorage.getItem("authToken");

  const handleIndtægt = async (income) => {
    try {
      const response = await fetch('http://localhost:5168/api/Findtægt', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`,

        },
        body: JSON.stringify(income),
      });
      console.log("Income", income);
      console.log("Response", response);
      if (!response.ok) {
        throw new Error('Noget gik galt. Kunne ikke gemme indkomst.');
      }
      
      const result = await response.json();
      return result.id;
      

    } catch (error) {
      console.error('Fejl:', error);
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
  <TextField
          label="Kategori"
          type="outlined"
          fullWidth
          value={kategoriNavn}
          onChange={(e) => setKategoriNavn(e.target.value)}
          margin="normal"
          InputLabelProps={{
            shrink: true, 
          }}
        />
<TextField
          label="Kateori ID"
          type="outlined"
          fullWidth
          value={kategoriId}
          onChange={(e) => setKategoriId(e.target.value)}
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
