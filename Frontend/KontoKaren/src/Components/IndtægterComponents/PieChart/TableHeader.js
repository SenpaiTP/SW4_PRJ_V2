import React from "react";
import {
  TableHead,
  TableRow,
  TableCell,
  Checkbox,
  IconButton,
} from "@mui/material";
import MoreVertIcon from "@mui/icons-material/MoreVert";

export default function TableHeader({
  numSelected,
  rowCount,
  onOpenMenu,
}) {
  return (
    <TableHead>
      <TableRow>
        {/* Checkbox for at vælge alle */}
        <TableCell padding="checkbox">
         
        </TableCell>

        {/* Kolonnehoveder */}
        <TableCell>Indtægtsnavn</TableCell>
        <TableCell align="left">Beløb</TableCell>
        <TableCell align="left">Dato</TableCell>
        <TableCell align="left">Handlinger</TableCell>
      </TableRow>
    </TableHead>
  );
}
