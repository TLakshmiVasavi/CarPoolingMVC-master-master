import React from "react";
import { Container, Row, Col } from "react-grid-system";
import { IMyOffers, IMyOffer, IRideRequests, IRideRequest } from "./Interfaces";
import { MdLocationOn } from "react-icons/md";
import { getRequests, getOffers } from "./Redux/Ride/RideActions";
import { connect } from "react-redux";
import { RouteComponentProps } from "react-router-dom";
import Modal from "./PopUp";
import RideRequests from "./RideRequests";
import { AppState } from "./Redux/rootReducer";
interface IPopUp {
  open: boolean;
}
class OfferedRides extends React.Component<IMyOffers & DispatchProps, IPopUp> {
  constructor(props: IMyOffers & DispatchProps) {
    super(props);
    this.state = {
      open: false,
    };
    this.handleClick = this.handleClick.bind(this);
    this.modalOpen = this.modalOpen.bind(this);
    this.modalClose = this.modalClose.bind(this);
  }
  componentWillMount() {
    this.props.getOffers();
  }
  handleClick(id: number) {
    this.props.getRequests(id);
    this.setState({ open: true });
  }
  modalOpen() {
    this.setState({ open: true });
  }

  modalClose() {
    this.setState({
      open: false,
    });
  }
  render() {
    return (
      <>
        {this.props.offers.map((ride: IMyOffer) => (
          <>
            <div
              className="shadowBox"
              onClick={() => this.handleClick(ride.id)}
            >
              <Row>
                <Col md={4}>
                  <small>From</small>
                  <p>{ride.from}</p>
                </Col>
                <Col md={4}>
                  <div className="dot" />
                  <div className="dot" />
                  <div className="dot" />
                  <MdLocationOn className="darkviolet" />
                </Col>
                <Col md={4}>
                  <small>To</small>
                  <p>{ride.to}</p>
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <small>Date</small>
                  <p>{ride.startDate}</p>
                </Col>
                <Col md={4} />
                <Col md={4}>
                  <small>Time</small>
                  <p>{ride.time}</p>
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <small>Price</small>
                  <p>{ride.cost}</p>
                </Col>
                <Col md={4} />
                <Col md={4}>
                  <small>Seats Available</small>
                  <p>{ride.noOfOfferedSeats}</p>
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <small>Ride Status</small>
                  <p>{ride.rideStatus}</p>
                </Col>
                <Col md={4} />
                <Col md={4}>
                  <small>Vehicle Number</small>
                  <p>{ride.vehicleNumber}</p>
                </Col>
              </Row>
            </div>
            <Modal show={this.state.open} handleClose={this.modalClose}>
              <RideRequests rideId={ride.id} />
            </Modal>
          </>
        ))}
      </>
    );
  }
}
interface DispatchProps {
  getOffers: () => void;
  getRequests: (rideId: number) => void;
}
const mapDispatchToProps = {
  getOffers,
  getRequests,
};

const mapStateToProps = (state: AppState) => ({
  offers: state.ride.offers,
});

export default connect(mapStateToProps, mapDispatchToProps)(OfferedRides);
