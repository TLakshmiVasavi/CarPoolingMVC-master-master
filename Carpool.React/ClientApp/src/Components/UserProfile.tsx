import React from "react";
import { Row, Col, Container } from "react-grid-system";
import { IUser } from "./Interfaces";
import { connect, DispatchProp } from "react-redux";
import { AppState } from "./Redux/rootReducer";
import { UpdateUser } from "./Redux/User/UserActions";
import { Redirect } from "react-router-dom";

class UserProfile extends React.Component<IProps, {}> {
  constructor(props: IProps) {
    super(props);
    this.enableEdit = this.enableEdit.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.disableEdit = this.disableEdit.bind(this);
  }

  enableEdit(e: any) {
    this.setState({ disable: false });
  }

  handleSubmit(e: any) {
    this.setState({ disable: true });
  }

  disableEdit(e: any) {
    this.setState({ disable: true });
  }

  render() {
    if (!this.props.isLoggedIn) {
      this.props.history.push("/Login");
    }

    return (
      <Container>
        <div className="shadowBox Profile center">
          <Row>
            <Col md={6}>Name</Col>
            <Col md={6}>
              <input
                type="text"
                value={this.props.user.name}
                disabled={this.props.disable}
              />
            </Col>
          </Row>
          <Row>
            <Col md={6}>Age</Col>
            <Col md={6}>
              <input
                type="number"
                value={this.props.user.age}
                disabled={this.props.disable}
              />
            </Col>
          </Row>
          <Row>
            <Col md={6}>Mail</Col>
            <Col md={6}>
              <input
                type="text"
                value={this.props.user.mail}
                disabled={this.props.disable}
              />
            </Col>
          </Row>
          <Row>
            <Col md={6}>Number</Col>
            <Col md={6}>
              <input
                type="text"
                value={this.props.user.number}
                disabled={this.props.disable}
              />
            </Col>
          </Row>
          <Row>
            <Col md={6}>Gender</Col>
            <Col md={6}>
              <select
                value={this.props.user.gender}
                disabled={this.props.disable}
              >
                <option value="Female">Female</option>
                <option value="Male">Male</option>
              </select>
            </Col>
          </Row>
          <Row>
            {this.props.disable ? (
              <button id="edit" value="edit" onClick={this.enableEdit}>
                edit
              </button>
            ) : (
              <React.Fragment>
                <button
                  type="submit"
                  id="save"
                  value="save"
                  onClick={this.handleSubmit}
                >
                  save
                </button>
                <button id="cancel" value="cancel" onClick={this.disableEdit}>
                  cancel
                </button>
              </React.Fragment>
            )}
          </Row>
        </div>
      </Container>
    );
  }
}
interface IProps extends DispatchProps, IUser {}

const mapStateToProps = (state: AppState) => ({
  user: state.user,
});

interface DispatchProps {
  updateUser: (user: IUser) => void;
}
export default connect(mapStateToProps, { UpdateUser })(UserProfile);
