import React, { useState, useEffect } from 'react';
import { Dialog, DialogActions, DialogContent, DialogTitle, Button, TextField } from '@mui/material';
import useIndtægterHooks from '../../../Hooks/IndtægterHooks.js';
import { initialRows } from '../Table/TableData';
import { useFetchAllIndtægt } from './FetchAllIndtægt.js';

export default function EditIncomeDialog({ open, handleClose}) {
  const {
    rows, // data i tabellen
    selected, // valgte rækker
    page, // sktuel side
    rowsPerPage, // antal rækker pr. side
    handleClick, // håndtering af klik på rækker
    handleAddRow, // tilføjelse af en ny række
    handleEditRow, // redigering af en række
    handleDeleteRow, // sletning af en række
    handleSave, // gemmer ændringer
    setRows, // opdaterer rækkerne
  } = useIndtægterHooks(initialRows);

  const [income, setIncome] = useState(null);
  const [row, setRow] = useState([]);
  const [FindtægtId, setFindtægtId] = useState('');
  const [name, setName] = useState('');
  const [price, setPrice] = useState('');
  const [date, setDate] = useState('');
  const [kategoriNavn, setKategoriNavn] = useState('');
  const [kategoriId, setKategoriId] = useState('');
  const token = localStorage.getItem("authToken");

  // const fetchIncomes = async () => {

  //   try {
  //     const response = await fetch('http://localhost:5168/api/Findtægt', {
  //       method: 'GET',
  //       headers: {
  //         'Authorization': `Bearer ${token}`,
  //         'content-type': 'application/json', 
  //       },
  //     });
  //     if (!response.ok) {
  //       throw new Error('Kunne ikke hente findtægter.');
  //     }
  
  //     const data = await response.json();
  //     const mappedData = mapData(data);
  //     console.log("Mapped data: ", mappedData);
  //     setIncome(mappedData);
  //     setRows(mappedData); // Opdaterer tabellen med data
  //   } catch (error) {
  //     console.error('Fejl:', error.message);
  //   }
  // };

  const handleIndtægt = async (updatedIncome) => {
    try {
      const response = await fetch('http://localhost:5168/api/Findtægt', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'content-type': 'application/json',
        },
      });
      if (!response.ok) {
        throw new Error('Kunne ikke hente findtægter.');
      }
  
      const data = await response.json();
      const mappedData = mapData(data);
      console.log("Mapped data: ", mappedData);
      setIncome(mappedData);
      setRows(mappedData); // Opdaterer tabellen med data
  
      // Find the specific income based on the updatedIncome.id
        try {
          const response = await fetch(`http://localhost:5168/api/Findtægt?findid=${FindtægtId}`, { // Use incomeToUpdate.id
            method: 'PUT',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${token}`,
            },
            body: JSON.stringify(updatedIncome),
          });
  
          console.log("Income", updatedIncome);
          console.log("Response", response);
  
          if (response.status === 204) {
            console.log('Indkomst opdateret succesfuldt.');
          } else if (response.ok) {
            const result = await response.json();
            console.log('Indkomst gemt:', result);
          } else {
            throw new Error('Noget gik galt. Kunne ikke gemme indkomst.');
          }
        } catch (error) {
          console.error('Fejl inde i fetchId:', error);
        }
    } catch (error) {
      console.error('Fejl i fetch all data:', error.message);
    }
  };

  useEffect(() => {
    //fetchIncomes();
  }, []);

  const mapData = (data) => {
    return data.map(item => ({
        id: item.findtægtId,
        Tekst: item.tekst,
        Indtægt: item.indtægt,
        Dato: item.dato,
        kategoriNavn: item.kategoriNavn,
        kategoriId: item.kategoriId,
    }));
};

  const handleSubmit = async (mapData) => {
    if ( FindtægtId && name && price && date && kategoriNavn && kategoriId) {
      const updatedIncome = {
        id: FindtægtId, // Use FindtægtId from state
        Tekst: name,
        Indtægt: price,
        Dato: date,
        KategoriNavn: kategoriNavn,
        KategoriId: kategoriId,
      };

      console.log("Rows", rows);
      console.log("Updated income", updatedIncome);
      // Find the specific row based on the selected row.id
      if(updatedIncome.id === FindtægtId) {
        await handleIndtægt(updatedIncome);
        handleSave(updatedIncome);
        handleClose()
      }
      else {
        alert('Den valgte række blev ikke fundet.');
      }
    } else {
      alert('Alle felter skal udfyldes!');
    }
  };

 

  return (
    <Dialog open={open} onClose={handleClose}>
      <DialogTitle>Rediger Indtægt</DialogTitle>
      <DialogContent>
      <TextField
          label="ID"
          variant="outlined"
          fullWidth
          value={FindtægtId}
          onChange={(e) => setFindtægtId(e.target.value)}
          sx={{ marginBottom: 2 }}
        />
        <TextField
          label="Indtægtsnavn"
          variant="outlined"
          fullWidth
          value={name}
          onChange={(e) => setName(e.target.value)}
          sx={{ marginBottom: 2 }}
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
