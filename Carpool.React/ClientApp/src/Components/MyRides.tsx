import React from "react";
import { Container, Row, Col } from "react-grid-system";
import { IAuthDetails } from "./Interfaces";
import { connect } from "react-redux";
import { AppState } from "./Redux/rootReducer";
import OfferedRides from "./OfferedRides";
import BookedRides from "./BookedRides";
import { Redirect, RouteComponentProps } from "react-router-dom";
interface IProps extends IAuthDetails, RouteComponentProps {}

class MyRides extends React.Component<IProps, {}> {
  render() {
    if (!this.props.isLoggedIn) {
      this.props.history.push("/Login");
    }

    return (
      <Container>
        <Row>
          <Col md={6}>
            <div className="rectangle bg-darkviolet">Booked Rides</div>
            <Col id="bookings" md={10}>
              <BookedRides />
            </Col>
          </Col>
          <Col md={6}>
            <div className="rectangle bg-darkorange">Offered Rides</div>
            <Col id="offers" md={10}>
              <OfferedRides />
            </Col>
          </Col>
        </Row>
      </Container>
    );
  }
}

const mapStateToProps = (state: AppState) => ({
  isLoggedIn: state.user.isLogedIn,
});
export default connect(mapStateToProps, null)(MyRides);
