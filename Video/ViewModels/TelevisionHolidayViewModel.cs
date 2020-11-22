using CommonBasicStandardLibraries.CollectionClasses;
using CommonBasicStandardLibraries.Exceptions;
using CommonBasicStandardLibraries.Messenging;
using CommonBasicStandardLibraries.MVVMFramework.UIHelpers;
using CommonBasicStandardLibraries.MVVMFramework.ViewModels;
using MediaHelpersCP.Video.DatabaseInterfaces.TVShows;
using MediaHelpersCP.Video.EventModels;
using MediaHelpersCP.Video.Logic;
using MediaHelpersCP.Video.TelevisionMiscClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MediaHelpersCP.Video.ViewModels
{
    public class TelevisionHolidayViewModel : Screen
    {
		//this will handle all the holiday stuff for the tv shows.

		private string _nonHolidayText = "";

		public string NonHolidayText
		{
			get { return _nonHolidayText; }
			set
			{
				if (SetProperty(ref _nonHolidayText, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _holidayFullVisible;

		public bool HolidayFullVisible
		{
			get { return _holidayFullVisible; }
			set
			{
				if (SetProperty(ref _holidayFullVisible, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _holidayFullText = "Full Hour";

		public string HolidayFullText
		{
			get { return _holidayFullText; }
			set
			{
				if (SetProperty(ref _holidayFullText, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private bool _holidayHalfVisible;

		public bool HolidayHalfVisible
		{
			get { return _holidayHalfVisible; }
			set
			{
				if (SetProperty(ref _holidayHalfVisible, value))
				{
					//can decide what to do when property changes
				}

			}
		}

		private string _holidayHalfText = "Half Hour";
		private readonly IEventAggregator _aggregator;
		private readonly ITelevisionHolidayLogic _logic;

		public string HolidayHalfText
		{
			get { return _holidayHalfText; }
			set
			{
				if (SetProperty(ref _holidayHalfText, value))
				{
					//can decide what to do when property changes
				}

			}
		}
		public TelevisionHolidayViewModel(IEventAggregator aggregator, ITelevisionHolidayLogic logic)
		{
			_aggregator = aggregator;
			_logic = logic;
		}
		public async Task LoadMainAsync()
		{
			await _aggregator.PublishAsync(new LoadTelevisionListEventModel());
		}
		public bool CanLoadHolidayEpisode(EnumTelevisionLengthType lengthType)
		{
			return lengthType switch
			{
				EnumTelevisionLengthType.HalfHour => HolidayHalfVisible,
				EnumTelevisionLengthType.FullHour => HolidayFullVisible,
				_ => throw new BasicBlankException("Only half an hour and full hour are supported.  Rethink"),
			};
		}
		public async Task LoadHolidayEpisodeAsync(EnumTelevisionLengthType lengthType)
		{
			var episodeList = _holidayList.GetConditionalItems(Items => Items.ShowTable.LengthType == lengthType);
			if (episodeList.Count == 0)
			{
				UIPlatform.ShowError("No episodes left.  Should have made the option invisible");
				return;
			}
			IEpisodeTable episode = episodeList.GetRandomItem();
			await _aggregator.PublishAsync(episode); //hopefully its this simple.
		}

		//needs to get holiday list.
		//i think another class to focus on holiday stuff (business logic).

		private CustomBasicList<IEpisodeTable> _holidayList = new CustomBasicList<IEpisodeTable>();
		private EnumTelevisionHoliday _currentHoliday;
		protected override async Task ActivateAsync()
		{
			try
			{
				_currentHoliday = DateTime.Now.WhichHoliday();
				_holidayList = await _logic.GetHolidayEpisodeListAsync();
				string p;
				NonHolidayText = $"Choose Shows With Non {_currentHoliday.ToString()} Episodes";
				if (_holidayList.Exists(items => items.ShowTable.LengthType == EnumTelevisionLengthType.FullHour) == false)
					HolidayFullVisible = false;
				else
				{
					p = HolidayFullText;
					HolidayFullText = $"{p} For {_currentHoliday.ToString()}";
					HolidayFullVisible = true;
				}
				if (_holidayList.Exists(items => items.ShowTable.LengthType == EnumTelevisionLengthType.HalfHour) == false)
					HolidayHalfVisible = false;
				else
				{
					p = HolidayHalfText;
					HolidayHalfText = $"{p} For {_currentHoliday.ToString()}";
					HolidayHalfVisible = true;
				}
				NotifyOfCanExecuteChange(nameof(CanLoadHolidayEpisode));
			}
			catch (Exception ex)
			{
				UIPlatform.ShowError(ex.Message);
			}
			
		}

	}
}
