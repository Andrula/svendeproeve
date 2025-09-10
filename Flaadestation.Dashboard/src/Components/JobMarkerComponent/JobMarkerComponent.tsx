import { useState, type FunctionComponent } from "react";
import "./JobMarkerComponent.css";
import { AdvancedMarker } from "@vis.gl/react-google-maps";
import classNames from "classnames";
import type { JobModel } from "../../Services/JobService";
import { format } from "date-fns";
import { Calendar } from "lucide-react";
import { ItemType } from "../../Constants/ItemType";

interface Props {
  job: JobModel;
  activeMarkerId: string | null;
  setMarkerJobId: (id: string | null) => void;
  setSelectedJob: (job:JobModel | null) => void;
  openModal: (itemType: ItemType) => void;
}

export const JobMarkerComponent: FunctionComponent<Props> = ({
  job,
  activeMarkerId: activeMarkerId,
  setMarkerJobId: setActiveMarkerId,
  setSelectedJob,
  openModal
}) => {
  const [hovered, setHovered] = useState(false);

  const position = {
    lat: job.data.address.y,
    lng: job.data.address.x,
  };

  const isClicked = activeMarkerId === job.data.jobId;

  const handleClick = () => {
    if (isClicked) {
      setSelectedJob(null)
      setActiveMarkerId(null);
    } else {
      setSelectedJob(job)
      setActiveMarkerId(job.data.jobId);
    }
  };
  

  const renderCustomPin = () => {
    return (
      <>
        <div className="custom-pin">
          <button className="close-button">
            <span className="material-symbols-outlined"> close </span>
          </button>

          <div className="image-container" onClick={handleClick}>
            {isClicked ? (
              <span className="icon">
                <i className="bi bi-x-lg"></i>
              </span>
            ) : (
              <span className="icon">
                <i className="bi bi-tools"></i>
              </span>
            )}
          </div>

          <div className="details-container">
            <div className="marker-content">
              <h2>{job.data.title}</h2>
              <p className="description">{job.data.description}</p>
              <div className="dates-section">
                <div className="date-range">
                  <Calendar size={16} />
                  <span>
                    {format(new Date(job.data.scheduledStart), "dd-MM-yyyy")} -{" "}
                    {format(new Date(job.data.scheduledEnd), "dd-MM-yyyy")}
                  </span>
                </div>
              </div>
            </div>

            <div className="tab-bar">
              <button className="tab-button" style={{borderRadius: "0 6px 0 0"}} onClick={() =>openModal(ItemType.Vehicle)}>
                <i className="bi bi-truck"></i>
                <br></br>
                <small className="tab-button-info fst-italic">
                  {job.data.storage.vehicles.length +
                    job.data.storage.defaultVehicles.length}
                </small>
              </button>
              <hr />
              <button className="tab-button" onClick={() =>openModal(ItemType.Employee)}>
                <i className="bi bi-person"></i>
                <br></br>
                <small className="tab-button-info fst-italic">
                  {job.data.storage.employees.length +
                    job.data.storage.defaultEmployees.length}
                </small>
              </button>
              <hr />
              <button className="tab-button" onClick={() =>openModal(ItemType.Tool)}>
                <i className="bi bi-wrench"></i>
                <br></br>
                <small className="tab-button-info fst-italic">
                  {job.data.storage.tools.length +
                    job.data.storage.defaultTools.length}
                </small>
              </button>
              <hr />
              <button className="tab-button" style={{borderRadius: "0 0 6px 0"}} onClick={() =>openModal(ItemType.Mahinery)}>
                <i className="bi bi-truck-front"></i>
                <br></br>
                <small className="tab-button-info fst-italic">
                  {job.data.storage.machines.length +
                    job.data.storage.defaultMachines.length}
                </small>
              </button>
            </div>
          </div>
        </div>

        <div className="tip" />
      </>
    );
  };

  return (
    <>
      <AdvancedMarker
        position={position}
        onMouseEnter={() => setHovered(true)}
        onMouseLeave={() => setHovered(false)}
        className={classNames("custom-job-marker", { clicked: isClicked, hovered })}
        zIndex={isClicked ? 1000 : hovered ? 500 : 1}
      >
        {renderCustomPin()}
      </AdvancedMarker>
    </>
  );
};
