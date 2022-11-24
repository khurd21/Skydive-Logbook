import React, { Component, Fragment } from 'react';
import { Link } from 'react-router-dom';
import authService from './AuthorizeService';
import { ApplicationPaths } from './ApiAuthorizationConstants';
import { MenuItem, Typography } from '@mui/material';

export class LoginMenu extends Component {
  constructor(props) {
    super(props);

    this.state = {
      isAuthenticated: false,
      userName: null
    };
  }

  componentDidMount() {
    this._subscription = authService.subscribe(() => this.populateState());
    this.populateState();
  }

  componentWillUnmount() {
    authService.unsubscribe(this._subscription);
  }

  async populateState() {
    const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])
    this.setState({
      isAuthenticated,
      userName: user && user.name
    });
  }

  render() {
    const { isAuthenticated, userName } = this.state;
    if (!isAuthenticated) {
      const registerPath = `${ApplicationPaths.Register}`;
      const loginPath = `${ApplicationPaths.Login}`;
      return this.anonymousView(registerPath, loginPath);
    } else {
      const profilePath = `${ApplicationPaths.Profile}`;
      const logoutPath = `${ApplicationPaths.LogOut}`;
      const logoutState = { local: true };
      return this.authenticatedView(userName, profilePath, logoutPath, logoutState);
    }
  }

  authenticatedView(userName, profilePath, logoutPath, logoutState) {
    const shortUserName = userName.split('@')[0];
    return (
      <Fragment>
        <MenuItem component={Link} to={profilePath}>
          <Typography textAlign='center'>Hello {shortUserName}</Typography>
        </MenuItem>
        <MenuItem component={Link} to={logoutPath} state={logoutState}>
          <Typography textAlign='center'>Logout</Typography>
        </MenuItem>
      </Fragment>
    );
  }

  anonymousView(registerPath, loginPath) {
    return (
    <Fragment>
      <MenuItem component={Link} to={loginPath}>Login</MenuItem>
    </Fragment>);
  }
}
