import React from "react";
import { Row, Col } from "react-grid-system";
import { AppState } from "./Redux/rootReducer";
import { MdLocationOn } from "react-icons/md";
import { IBookRide, IBookRideResponse, IAvailableRide } from "./Interfaces";
import { connect } from "react-redux";
import { requestRide } from "./Redux/Ride/RideActions";
import "../App.css";
import Modal from "./PopUp";

interface IState {
  NumberOfSeats: number;
  modal: boolean;
  isImageOpen: boolean;
}
class AvailableRides extends React.Component<IProps, IState> {
  constructor(props: IProps) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this);
    this.state = {
      modal: false,
      NumberOfSeats: 0,
      isImageOpen: false,
    };
  }

  modalOpen() {
    this.setState({ modal: true });
  }

  modalClose() {
    this.setState({
      NumberOfSeats: 0,
      modal: false,
    });
  }

  openImage() {
    this.setState({ isImageOpen: true });
  }

  closeImage() {
    this.setState({ isImageOpen: false });
  }

  handleSubmit(rideId: number) {
    this.props.requestRide(this.props, this.state.NumberOfSeats, rideId);
  }

  render() {
    var url = "data:image/png;base64,";
    return (
      <React.Fragment>
        {this.props.isLoaded && this.props.availableRides.length == 0 ? (
          <div>Rides Not Available</div>
        ) : (
          this.props.availableRides.map((item: IAvailableRide) => (
            <div className="shadowBox" onClick={this.modalOpen}>
              <Modal
                show={this.state.modal}
                handleClose={() => this.modalClose()}
              >
                <div className="form-group">
                  <label>Enter Number Of Seats:</label>
                  <input className="form-control" />
                </div>
                <div className="form-group">
                  <button
                    type="button"
                    onClick={() => this.handleSubmit(item.id)}
                  >
                    Request
                  </button>
                </div>
              </Modal>
              <Modal
                show={this.state.isImageOpen}
                handleClose={this.closeImage}
              >
                <img src={url + item.providerPic} onClick={this.closeImage} />
              </Modal>
              <Row>
                <Col md={8}>
                  <h2>{item.providerName}</h2>
                </Col>
                <Col md={4}>
                  <img
                    src={url + item.providerPic}
                    className="imgRound"
                    onClick={this.openImage}
                  />
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <small>From</small>
                  <p>{this.props.from}</p>
                </Col>
                <Col md={4}>
                  <div className="dot" />
                  <div className="dot" />
                  <div className="dot" />
                  <MdLocationOn className="darkviolet" />
                </Col>
                <Col md={4}>
                  <small>To</small>
                  <p>{this.props.to}</p>
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <small>Date</small>
                  <p>
                    {/* {new Date(this.props.startDate.toLocaleDateString())} */}
                    {this.props.startDate.toDateString()}
                  </p>
                </Col>
                <Col md={4} />
                <Col md={4}>
                  <small>Time</small>
                  <p>{this.props.time}</p>
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <small>Price</small>
                  <p>{item.cost}</p>
                </Col>
                <Col md={4} />
                <Col md={4}>
                  <small>Seats Available</small>
                  <p>{item.availableSeats}</p>
                </Col>
              </Row>
              <Row>
                <Col md={4}>
                  <small>Vehicle </small>
                  <p>{item.vehicle.model}</p>
                </Col>
                <Col md={4} />
                <Col md={4}>
                  <small>Vehicle Number</small>
                  <p>{item.vehicle.number}</p>
                </Col>
              </Row>
            </div>
          ))
        )}
      </React.Fragment>
    );
  }
}
const mapStateToProps = (state: AppState) => ({
  isLoaded: state.ride.isLoaded,
  availableRides: state.ride.availableRides,
});

interface IProps extends DispatchProps, IBookRideResponse, IBookRide {
  isLoaded: boolean;
}

interface DispatchProps {
  requestRide(Request: IBookRide, noOfSeats: number, rideId: number): void;
}

export default connect(mapStateToProps, { requestRide })(AvailableRides);
