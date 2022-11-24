import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService';
import { Tabs, Tab, TabPanel, TabList } from 'react-tabs';
import { JumpType } from './jump-info/JumpType';
import 'react-tabs/style/react-tabs.css';

const logbookApi = 'api/v1/logbook';

export class Logbook extends Component {
    static displayName = Logbook.name;

    constructor(props) {
        super(props);
        this.state = {
            logbookEntries: [],
            loading: true,
            userName: null
        };
    }

    componentDidMount() {
        this.setUserName();
        this.populateLogbookData();
    }

    async setUserName() {
        const [user] = await Promise.all([authService.getUser()])
        this.setState({
            userName: user && user.name
        });
    }

    async populateLogbookData() {
        const token = await authService.getAccessToken();
        const response = await fetch(logbookApi + '?From=1&To=10', {
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
        });
        const data = await response.json();
        this.setState({ logbookEntries: data.jumps, loading: true });
    }

    static renderLogJumpForm(logbookEntries) {
        return (
            <form>
                <label>
                    Date:
                    <input type="text" name="date" defaultValue={new Date().toISOString().slice(0, 10)} />
                    Jump Number:
                    <input type="text" name="jumpNumber" defaultValue={logbookEntries.length + 1}/>
                    Jump Type:
                    <select name="jumpType">
                        <option value="Tandem">Tandem</option>
                        <option value="Static Line">Static Line</option>
                        <option value="Accelerated Free Fall">Accelerated Free Fall</option>
                    </select>
                </label>
                <input className="btn btn-primary" type="submit" value="Submit" />
            </form>
        );
    }

    static renderLogbookTable(logbookEntries) {

        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Jump Number</th>
                        <th>Jump Type</th>
                    </tr>
                </thead>
                <tbody>
                    {logbookEntries.map(logbookEntry =>
                        <tr key={logbookEntry.jumpNumber}>
                            <td>{logbookEntry.date}</td>
                            <td>{logbookEntry.jumpNumber}</td>
                            <td>{JumpType.jumpTypeToString(logbookEntry.jumpCategory)}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        return (
            <div>
                <h1>Logbook</h1>
                <p>This is the logbook page for {this.state.userName}.</p>
                <p aria-live="polite">Current count: <strong>{this.state.logbookEntries.length}</strong></p>
                <Tabs>
                    <TabList>
                        <Tab>List Jumps</Tab>
                        <Tab>Log Jumps</Tab>
                    </TabList>
                    <TabPanel>
                        {Logbook.renderLogbookTable(this.state.logbookEntries || [])}
                    </TabPanel>
                    <TabPanel>
                        {Logbook.renderLogJumpForm(this.state.logbookEntries || [])}
                    </TabPanel>
                </Tabs>
            </div>
        );
    }
}