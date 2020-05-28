import React, { Component } from 'react';
import LoginForm from './LoginForm';
import SignUpForm from './SignUpForm';
import '../StyleSheets/LoginSignUp.css';
import { BrowserRouter as Router, Route, Link, Switch } from 'react-router-dom';

class LoginSignUp extends Component<{},{}> {

  render() {
    return (
      <React.Fragment>
        <div className="leftHalf">
          <div className="heading">TURN<div className="miles"> MILES</div><br />INTO <div className="money">MONEY</div><div className="normal">RIDES ON TAP</div></div>
        </div>
        <Router>
          <Switch>
            <Route exact path="/SignUp" component={SignUpForm} />
            <Route exact path="/(Login|)/" component={LoginForm} />
          </Switch>
        </Router>
      </React.Fragment>
    );
  }
}

export default LoginSignUp;