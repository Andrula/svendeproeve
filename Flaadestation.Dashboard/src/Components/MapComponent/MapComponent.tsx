import "./MapComponent.css";
import { useEffect, useState } from "react";
import { APIProvider, Map } from "@vis.gl/react-google-maps";
import { JobModel } from "../../Models/Job";
import { jobService } from "../../Services/JobService";
import { dawaService } from "../../Services/DawaService";
import { JobMarkerComponent } from "../JobMarkerComponent/JobMarkerComponent";
import type { BaseModel } from "../../Models/Base";
import { baseService } from "../../Services/BaseService";
import { BaseMarkerComponent } from "../BaseMarkerComponent/BaseMarkerComponent";


export default function GoogleMap() {
  const [jobs, setJobs] = useState<JobModel[]>([]);
  const [bases, setBases] = useState<BaseModel[]>([]);
  const [activeMarkerId, setActiveMarkerId] = useState<string | null>(null);
  const apiKey = import.meta.env.VITE_GOOGLE_MAPS_API_KEY as string;

  useEffect(() => {
    loadJobs();
  }, []);

  const loadJobs = async () => {
    const jobModels = await jobService.getJobsByCompany();
    await dawaService.fillAddressesOnJobs(jobModels);
    setJobs(jobModels);

    const baseModels = await baseService.getBasesByCompany();
    await dawaService.fillAddressesOnBases(baseModels);
    setBases(baseModels);
  };

  const denmarkBounds = {
    north: 57.85,
    south: 54.26,
    west: 7.90,
    east: 15.26
  }

  return (
    <div className="map-container">
      <APIProvider apiKey={apiKey}>
        <Map
          restriction={{
            latLngBounds: denmarkBounds,
            strictBounds: false,
          }}
          mapId={"1"}
          defaultCenter={{ lat: 56.26392, lng: 9.501785 }}
          defaultZoom={6}
          style={{
            height: "100%",
            width: "100%",
          }}
        >

          {jobs.map((job) => (
            <JobMarkerComponent
              key={job.data.jobId}
              job={job}
              activeMarkerId={activeMarkerId}
              setMarkerJobId={setActiveMarkerId}
            />
          ))}

          {bases.map((base) => (
            <BaseMarkerComponent
              key={base.data.baseId}
              base={base}
              activeMarkerId={activeMarkerId}
              setMarkerBaseId={setActiveMarkerId}
            />
          ))}
        </Map>
      </APIProvider>
    </div>
  );
}
