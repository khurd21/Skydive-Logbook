import React, { useState, useEffect } from 'react';
import { Box, Button, Grid, Paper, TableContainer, Table, TableHead, TableRow, TableCell, Typography, TableBody, TextField } from '@mui/material';
import 'react-tabs/style/react-tabs.css';
import authService from './api-authorization/AuthorizeService';
import { borderRight } from '@mui/system';

const logbookApi = 'api/v1/logbook';

const columns = [
    { id: 'jumpNumber', label: 'Jump Number', minWidth: 170 },
    { id: 'date', label: 'Date', minWidth: 100 },
    { id: 'dropzone', label: 'Dropzone', minWidth: 170 },
    { id: 'jumpCategory', label: 'Jump Category', minWidth: 170 },
];

async function retrieveLoggedJumps() {

    const token = await authService.getAccessToken();
    const response = await fetch(logbookApi, {
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    console.log(`retrieve logged jumps : `);
    console.log(response);

    return response;
}

async function deleteJump(jump) {

    const token = await authService.getAccessToken();
    const response = await fetch(`${logbookApi}?jumpNumber=${jump.jumpNumber}`, {
        method: 'DELETE',
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });

    if (response.ok) {
        console.log(`jump deleted : `);
        console.log(response);
    }
    else {
        console.log(`jump delete failed : `);
        const error = await response.json();
        console.log(error);
    }

    return response;
}

async function logJump(jump) {

    const token = await authService.getAccessToken();
    const response = await fetch(logbookApi, {
        method: 'POST',
        headers: !token ? {}
            : { 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json' },
        body: JSON.stringify(jump)
    })
    .catch(error => {
        console.log('FOUND ERROR');
        console.error(error);
    });

    if (response.ok) {
        console.log('jump logged');
    }
    else {
        console.log('jump not logged');
        const error = await response.json();
        console.log(error);
    }

    return response;
}

export default function Logbook() {

    const [editJumpDisabled, setEditJumpDisabled] = useState(false);

    const [deleteJumpDisabled, setDeleteJumpDisabled] = useState(false);

    const [loggedJumps, setLoggedJumps] = useState([]);

    const [jumpToEditOrLog, setJumpToEditOrLog] = useState({});

    useEffect(() => {
        retrieveLoggedJumps().then(response => response.json()).then(data => {
            setLoggedJumps(data);
        });
    }, []);

    console.log(`logged jumps: ${loggedJumps}`);
    console.log(loggedJumps);

    return (
        <Box sx={{ padding: 15 }}>
            <Grid container spacing={0} style={{ minHeight: '100vh'}}>

                <Grid item xs={3}>
                    <Button>New Jump</Button>
                    <Button disabled={editJumpDisabled}>Edit</Button>
                    <Button disabled={deleteJumpDisabled} onClick={
                        () => { deleteJump({jumpNumber: 4}, 'DELETE'); }
                    }>Delete</Button>
                </Grid>

                <Grid item style={{ flexGrow: "1" }} sx={{ padding: 2 }}>
                    <Paper sx={{ width: '100%', overflow: 'auto'}}>
                        <TableContainer sx={{ maxHeight: 440}}>
                            <Table stickyHeader>
                                <TableHead>
                                    <TableRow>
                                        {columns.map((column) => (
                                            <TableCell
                                                key={column.id}
                                                align={column.align
                                                    ? column.align
                                                    : 'left'}
                                                style={{ minWidth: column.minWidth }}
                                            >
                                                {column.label}
                                            </TableCell>
                                        ))}
                                    </TableRow>
                                </TableHead>
                                <TableBody>
                                    {loggedJumps.jumps ? loggedJumps.jumps.map((jump) => (
                                        <TableRow hover role="checkbox" tabIndex={-1} key={jump.jumpNumber}>
                                            {columns.map((column) => {
                                                const value = jump[column.id];
                                                return (
                                                    <TableCell key={column.id} align={column.align}>
                                                        {column.format && typeof value === 'number'
                                                            ? column.format(value)
                                                            : value ? value : 'N/A'}
                                                    </TableCell>
                                                );
                                            })}
                                        </TableRow>
                                    )) : null}
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </Paper>
                </Grid>
                <Grid xs={3} item sx={{
                    padding: 2, gap: 2, display: 'flex', flexDirection: 'column', justifyContent: 'space-between'
                }}>
                    {console.log(`jump to edit or log: ${jumpToEditOrLog}`)}
                    {console.log(jumpToEditOrLog)}
                    <TextField required id='jumpNumber' label='Jump Number' variant='outlined' onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, jumpNumber: e.target.value })} />
                    <TextField required id='date' label='Date' variant='outlined' defaultValue={new Date().toISOString().slice(0, 10)} onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, date: e.target.value })} value={jumpToEditOrLog.date} />
                    <TextField id='dropzone' label='Dropzone' variant='outlined' onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, dropzone: e.target.value })} />
                    <TextField id='jumpCategory' label='Jump Category' variant='outlined' onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, jumpCategory: e.target.value })} />
                    <TextField id='aircraft' label='Aircraft' variant='outlined' onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, aircraft: e.target.value })} />
                    <TextField id='altitude' label='Altitude' variant='outlined' onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, altitude: e.target.value })} />
                    <TextField id='parachute' label='Parachute' variant='outlined' onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, parachute: e.target.value })} />
                    <TextField id='parachuteSize' label='Parachute Size' variant='outlined' onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, parachuteSize: e.target.value })} />
                    <TextField multiline id='description' label='Description' variant='outlined' onChange={(e) => setJumpToEditOrLog({ ...jumpToEditOrLog, description: e.target.value })} />
                    <Button onClick={() => logJump({jumpNumber: 4})}>Log Jump</Button>
                </Grid>
            </Grid>
        </Box>
    );
}
